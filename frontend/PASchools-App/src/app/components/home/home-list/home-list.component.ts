import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { School } from '@app/models/school';
import { CoordinateInterface } from '@app/models/coordinateInterface';
import { Coordinate } from '@app/models/coordinate';
import { CommonService } from '@app/services/common.service';
import { HomeService } from '@app/services/home.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map, startWith, take } from 'rxjs/operators';
import { Address } from '@app/models/address';
import { Route } from '@app/models/route';
import { PageConfig } from '@app/models/pageConfig';
import { PagedObject } from '@app/models/pagedObject';

@Component({
  selector: 'app-home-list',
  templateUrl: './home-list.component.html',
  styleUrls: ['./home-list.component.scss']
})
export class HomeListComponent implements OnInit {
  public schools: School[] = [];
  public currentCoordinate?: Coordinate;
  public destinationCoordinate?: Coordinate;
  public destSchoolName?: string;
  public address: Address = {} as Address;
  public currentRoute: Route = {} as Route;
  public filteredSchools: School[] = [];
  public _filterRows : string = "";
  public modalRef?: BsModalRef;
  public myControl = new FormControl();


  pageConfig: PageConfig = {
    length: 0,
    pageIndex:0,
    pageSize:10,
    pageSizeOptions: []
  };

  mapOptions: google.maps.MapOptions = {
    center: {lat: 0, lng: 0},
    zoom: 12,
  };

  markerOptions: google.maps.MarkerOptions = {};

  form!: FormGroup;
  public get f(){
    return this.form.controls;
  }

  public config = {
    search:true,
    height: 'auto',
    noResultsFound: 'Sem resultados',
    placeholder:'Selecione',
    searchPlaceholder:'Procurar...'

  }

  public options = new Array();
  filteredOptions = {} as  Observable<any[]>;

  constructor(private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private homeService: HomeService,
    private fb: FormBuilder,
    private commonService: CommonService
    ) { }

    public get filterRows(){
      return this._filterRows
    }
    public set filterRows(value){
      this._filterRows = value;
      this.filteredSchools = this._filterRows ? this.filterSchools(this._filterRows) : this.schools;
    }

    ngOnInit(): void {
      this.validate();
      this.filteredOptions = this.myControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value)),
        );

      }

      getSchools(): void{
        this.address = { ...this.form.value };
        this.spinner.show();
        this.clearTable();

        this.homeService.postGetCoordinates(this.address!).subscribe({
          next: (_coord: Coordinate) => {
            this.currentCoordinate = _coord;
            this.getOrdered();
          },
          error: (e) => {
            this.spinner.hide();
            this.toastr.error("Erro ao encontrar as coordenadas do endereço informado." + e, "Erro");
            return;
          }
        });
      }


      getOrdered(): void{
        this.homeService.getSchoolsOrdered(this.currentCoordinate!).subscribe({
          next: (_schools: School[]) => {
            this.pageConfig.length = _schools.length;
            this.pageConfig.pageSizeOptions = [5,10,25,100, _schools.length];

            this.schools =  _schools;
            this.page(0, this.pageConfig.pageSize);

            this.spinner.hide();
          },
          error: (e) => {
            this.spinner.hide();
            this.toastr.error("Erro ao carregar as escolas." + e, "Erro");
            return;
          }
        });
      }

      public clearTable(): void{
        this.schools =  [];
      }

      openModalMaps(template: TemplateRef<any>) {
        this.mapOptions.center = {lat: this.currentCoordinate!.lat, lng:this.currentCoordinate!.lng}
        this.modalRef = this.modalService.show(template, {class: 'modal-lg'});

      }

      showSpotDetails(spot: any, infoWindow: any): void{
        console.log(spot);
        console.log(infoWindow);
        let selected = this.schools.filter((sch : School) => sch.code == spot.id)[0];
        infoWindow.open(spot);
      }


      openModal(template: TemplateRef<any>, destination: School) {
        this.destinationCoordinate = new Coordinate(destination.address.latitude, destination.address.longitude);
        this.destSchoolName = destination.name;
        this.spinner.show();
        this.homeService.postGetRoute(this.currentCoordinate!, this.destinationCoordinate).subscribe({
          next: (_route: Route) => {
            this.currentRoute = _route;
            this.toastr.success("Rota para " + this.destSchoolName + ", carregada!", "Sucesso");
            this.modalRef = this.modalService.show(template, {class: 'modal-lg'});
            this.spinner.hide();
          },
          error: (e) => {
            this.spinner.hide();
            this.toastr.error("Erro ao encontrar as coordenadas do endereço informado." + e, "Erro");
          }
        });
      }

      confirm(): void {
        this.modalRef?.hide();
      }

      decline(): void {
        this.modalRef?.hide();
      }

      private _filter(value: string): string[] {
        const filterValue = value.toLowerCase();
        return this.options.filter(option => option.toLowerCase().includes(filterValue));
      }

      public filterSchools(filter : string) : School[] {
        filter = filter.toLocaleLowerCase();

        return this.schools.filter((sch : School) => sch.name.toLocaleLowerCase().indexOf(filter) !== -1)
      }

      public validate(): void{
        this.form = this.fb.group({
          street:[this.address?.street, Validators.required],
          number:[this.address?.number, Validators.required],
          district:[this.address?.district, Validators.required]
        });

      }

      public clearForm(event: any): void{
        event.preventDefault();
        this.form.reset();
      }

      public handlePageEvent(event: any): void{
        this.page(event.pageIndex, event.pageSize);
      }

      page(index: number, size : number): void{
        const schoolsAux = this.schools.slice();
        this.filteredSchools = schoolsAux.splice(index * size, size);
      }

    }

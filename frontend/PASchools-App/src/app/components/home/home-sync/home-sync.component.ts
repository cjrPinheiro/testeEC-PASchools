import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { SyncResponse } from '@app/models/syncResponse';
import { CommonService } from '@app/services/common.service';
import { HomeService } from '@app/services/home.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-home-sync',
  templateUrl: './home-sync.component.html',
  styleUrls: ['./home-sync.component.scss']
})
export class HomeSyncComponent implements OnInit {

  public qttImports: number = 317;
  public progressValue: number = 0;
  public modalRef?: BsModalRef;
  public showLoadComplete = false;
  public templateRef = {} as TemplateRef<any>;
  constructor(private spinner: NgxSpinnerService, private toastr: ToastrService,
     private homeService: HomeService, private modalService: BsModalService, private commonService: CommonService, private router: Router) { }

  ngOnInit() {
    document.getElementById('btInit')?.click();
  }
  async openModal(template: TemplateRef<any>){
    this.progressValue = 0;
    this.modalRef = this.modalService.show(template, {class: 'modal-md modal-dialog-centered'});
    this.SyncProjects();
  }

  private SyncProjects(): void{
    this.homeService.syncSchools(this.qttImports).subscribe({
      next: (_resp: SyncResponse) => {
        this.toastr.success(`${_resp.total} registros foram atualizados/importados!.`);
        this.progressValue = 100;
        this.showLoadComplete = true;
      },
      error: (e) => {
          this.commonService.handleHttpErrorResponse(e);
      }
    });
  }

  close(): void {
    this.modalRef?.hide();
    this.router.navigateByUrl('/home/list');
  }
}

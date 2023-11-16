import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";

import {AutoCompleteModule} from 'primeng/autocomplete';
import {TabViewModule} from 'primeng/tabview';

import { AppRoutingModule } from '@app/app-routing.module';
import { AppComponent } from '@app/app.component';
import { NavComponent } from '@app/shared/nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeService } from '@app/services/home.service';
import { DateTimeFormatPipe } from '@app/helpers/dateTimeFormat.pipe';
import { TitleComponent } from '@app/shared/title/title.component';
import { SelectDropDownModule } from 'ngx-select-dropdown'
import {DropdownModule} from 'primeng/dropdown';
import { GoogleMapsModule } from '@angular/google-maps'

import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule }  from '@angular/material/select';
import {MatExpansionModule} from '@angular/material/expansion';
import { HomeComponent } from './components/home/home.component';
import { HomeListComponent } from './components/home/home-list/home-list.component';
import { HomeSyncComponent } from './components/home/home-sync/home-sync.component';

import ptBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
registerLocaleData(ptBr);


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    DateTimeFormatPipe,
    TitleComponent,
    HomeComponent,
    HomeListComponent,
    HomeSyncComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CollapseModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule,
    TooltipModule,
    ModalModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule,
    MatTabsModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatExpansionModule,
    SelectDropDownModule,
    ProgressbarModule,
    AutoCompleteModule,
    TabViewModule,
    DropdownModule,
    GoogleMapsModule
  ],
  providers: [
    HomeService,
    BsModalService,
    { provide: LOCALE_ID, useValue: 'pt' }
  ],
  bootstrap: [AppComponent],

})
export class AppModule { }

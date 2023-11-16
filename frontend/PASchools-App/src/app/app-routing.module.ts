import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { HomeListComponent } from './components/home/home-list/home-list.component';
import { HomeSyncComponent } from './components/home/home-sync/home-sync.component';


const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path:'',
    runGuardsAndResolvers: 'always',
    children:[
      { path: 'home', redirectTo: 'home/list'},

      {
        path: 'home', component: HomeComponent,
        children: [
          { path: 'list', component: HomeListComponent },
          { path: 'sync', component: HomeSyncComponent }
        ]
      },
    ],
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

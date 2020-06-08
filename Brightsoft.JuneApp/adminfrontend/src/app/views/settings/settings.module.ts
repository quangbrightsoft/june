import { UserEditComponent } from './user-edit.component';
// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

// Theme Routing
import { SettingRoutingModule } from './settings-routing.module';
import { UserComponent } from './user.component';
import { CdkTableModule } from '@angular/cdk/table';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BaseRoutingModule } from '../base/base-routing.module';

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    SettingRoutingModule,
    CdkTableModule,
    ReactiveFormsModule,
    PaginationModule.forRoot(),
  ],
  declarations: [
    UserComponent,
    UserEditComponent
  ]
})
export class SettingsModule { }

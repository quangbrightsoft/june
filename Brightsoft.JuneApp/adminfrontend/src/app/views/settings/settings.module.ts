import { FormlyModule } from '@ngx-formly/core';
import { ControlMessagesComponent } from './control-messages.component';
import { AlertModule } from 'ngx-bootstrap/alert';
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

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    SettingRoutingModule,
    CdkTableModule,
    ReactiveFormsModule,
    PaginationModule.forRoot(),
    AlertModule,
    FormlyModule
  ],
  declarations: [
    UserComponent,
    UserEditComponent,
    ControlMessagesComponent
  ]
})
export class SettingsModule { }

import { UserEditComponent } from './user-edit.component';
// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

// Theme Routing
import { SettingRoutingModule } from './settings-routing.module';
import { UserComponent } from './user.component';
import { CdkTableModule } from '@angular/cdk/table';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    SettingRoutingModule,
    CdkTableModule,
    ReactiveFormsModule
  ],
  declarations: [
    UserComponent,
    UserEditComponent
  ]
})
export class SettingsModule { }

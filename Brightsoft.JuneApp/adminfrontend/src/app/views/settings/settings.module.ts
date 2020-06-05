// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

// Theme Routing
import { SettingRoutingModule } from './settings-routing.module';
import { UsersComponent } from './users.component';
import { CdkTableModule } from '@angular/cdk/table';

@NgModule({
  imports: [
    CommonModule,
    SettingRoutingModule,
    CdkTableModule
  ],
  declarations: [
    UsersComponent
  ]
})
export class SettingsModule { }

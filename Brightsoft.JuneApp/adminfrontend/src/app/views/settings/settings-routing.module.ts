import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UsersComponent } from './users.component';
import { UserEditComponent } from './user-edit.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Settings'
    },
    children: [
      {
        path: '',
        redirectTo: 'users'
      },
      {
        path: 'users',
        component: UsersComponent,
        data: {
          title: 'User Management'
        }
      },
      {
        path: 'users/edit/:id',
        component: UserEditComponent,
        data: {
          title: 'User Edit'
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingRoutingModule {}

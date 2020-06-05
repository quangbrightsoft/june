import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { UserComponent } from "./user.component";
import { UserEditComponent } from "./user-edit.component";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Settings",
    },
    children: [
      {
        path: "",
        redirectTo: "user",
      },
      {
        path: "user",
        component: UserComponent,
        data: {
          title: "User Management",
        },
      },
      {
        path: "user-edit/:id",
        component: UserEditComponent,
        data: {
          title: "User Edit",
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SettingRoutingModule {}

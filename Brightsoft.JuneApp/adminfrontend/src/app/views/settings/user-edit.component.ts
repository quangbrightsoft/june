import { UserModel } from "./../../../models/userModel";
import { UserService } from "./../../user.service";
import { Component } from "@angular/core";
import { DataSource } from "@angular/cdk/table";
import { Observable, BehaviorSubject } from "rxjs";

@Component({
  templateUrl: "users.component.html",
})
export class UserEditComponent {
 
  constructor(private userService: UserService) {}
  ngOnInit() {
  }
  getCurrentUser() {
    
  }
  
}
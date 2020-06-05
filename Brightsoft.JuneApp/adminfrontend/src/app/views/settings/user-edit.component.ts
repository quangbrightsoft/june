import { UserModel } from "./../../../models/userModel";
import { UserService } from "./../../user.service";
import { Component } from "@angular/core";
import { DataSource } from "@angular/cdk/table";
import { Observable, BehaviorSubject } from "rxjs";
import { FormGroup, FormBuilder } from "@angular/forms";
import { JWTTokenService } from "../../jwt-token.service";
import { Router } from "@angular/router";

@Component({
  templateUrl: "user-edit.component.html",
})
export class UserEditComponent {
  editForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private jWTTokenService: JWTTokenService,
    private router: Router
  ) {
    this.editForm = this.formBuilder.group({
      fullName: "",
      email: "",
      roles: [],
    });
  }
  ngOnInit() {}
  getCurrentUser() {}
  onSubmit() {
    this.userService.create(this.editForm.value).subscribe((result) => {
      console.log(result);
    });
  }
}

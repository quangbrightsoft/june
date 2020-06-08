import { UserModel } from "./../../../models/userModel";
import { UserService } from "./../../user.service";
import { Component } from "@angular/core";
import { DataSource } from "@angular/cdk/table";
import { Observable, BehaviorSubject } from "rxjs";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import { JWTTokenService } from "../../jwt-token.service";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastService } from "angular-toastify";

@Component({
  templateUrl: "user-edit.component.html",
})
export class UserEditComponent {
  editForm: FormGroup;
  currentUserId: string;
  currentUser: UserModel;
  availableRoles: string[] = ["Admin", "PowerUser"];
  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private jWTTokenService: JWTTokenService,
    private router: Router,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) {
    this.editForm = this.formBuilder.group({
      fullName: "",
      email: "",
      roles: this.formBuilder.group({}),
    });
  }
  ngOnInit() {
    this.route.params.subscribe((p) => {
      this.currentUserId = p && p["id"];
      parseInt(this.currentUserId) &&
        this.userService.get(this.currentUserId).subscribe((response) => {
          this.currentUser = response.data.user;
          this.editForm.value.fullName = this.currentUser.userName;
        });
    });
    const checkboxes = <FormGroup>this.editForm.get("roles");
    this.availableRoles.forEach((option: any) => {
      checkboxes.addControl(option, new FormControl(false));
    });
  }
  getCurrentUser() {}
  onSubmit() {
    let params = {
      id: this.currentUserId,
      fullName: this.editForm.value.fullName,
      email: this.editForm.value.email,
      roles: this.rolesFormGroupSelectedIds,
    };
    if (!this.currentUserId) {
      this.userService.create(params).subscribe(
        (result) => {
          this.goTo("/settings/user");
          this.addInfoToast("success!");
        },
        (err) => {
          this.addInfoToast("err!");
        }
      );
    } else {
      this.userService.edit(params).subscribe(
        (result) => {
          this.goTo("/settings/user");
          this.addInfoToast("success!");
        },
        (err) => {
          this.addInfoToast("err!");
        }
      );
    }
  }
  addInfoToast(message) {
    this.toastService.info(message);
  }
  goTo(route: string) {
    this.router.navigate([route]).catch(console.log);
  }
  get rolesFormGroupSelectedIds(): string[] {
    let ids: string[] = [];
    for (var key in this.editForm.value.roles) {
      if (this.editForm.value.roles[key]) {
        ids.push(key);
      }
    }
    return ids;
  }
}

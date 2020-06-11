import { UserModel } from "./../../../models/userModel";
import { UserService } from "./../../user.service";
import { Component } from "@angular/core";
import { DataSource } from "@angular/cdk/table";
import { Observable, BehaviorSubject } from "rxjs";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  FormArray,
  Validators,
} from "@angular/forms";
import { JWTTokenService } from "../../jwt-token.service";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastService } from "angular-toastify";

@Component({
  templateUrl: "user-edit.component.html",
})
export class UserEditComponent {
  editForm: FormGroup;
  currentUserId: string;
  currentUser: UserModel = { roles: [] } as UserModel;
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
      userName: "",
      fullName: "",
      email: ["", Validators.required],
      createdAt: "",
      roles: [],
      roleControls: this.formBuilder.array([]),
      deletedAt: "",
      id: "",
      isDisabled: false,
      isDeleted: false,
      ssn: "",
      updatedAt: "",
    });
  }
  ngOnInit() {
    this.route.params.subscribe((p) => {
      this.currentUserId = p && p["id"];
      if (parseInt(this.currentUserId)) {
        this.userService.get(this.currentUserId).subscribe((response) => {
          this.currentUser = response.data.user;
          this.editForm.patchValue(this.currentUser);
          this.loadRoles();
        });
      } else {
        this.loadRoles();
      }
    });
  }
  loadRoles() {
    let currentUser = this.currentUser;
    this.availableRoles.forEach((currentRole, i) => {
      const control = new FormControl(currentUser.roles.includes(currentRole)); // if first item set to true, else false
      (this.editForm.controls.roleControls as FormArray).push(control);
    });
  }
  onSubmit() {
    if (this.editForm.dirty && this.editForm.valid) {
      const selectedroles = this.editForm.value.roleControls
        .map((v, i) => (v ? this.availableRoles[i] : null))
        .filter((v) => v !== null);
      let params = {
        id: this.currentUserId,
        fullName: this.editForm.value.fullName,
        email: this.editForm.value.email,
        roles: selectedroles,
      };
      if (!parseInt(this.currentUserId)) {
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
    } else {
      console.log("invalid form!");
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

import { Component } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { UserService } from "../../user.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "register.component.html",
})
export class RegisterComponent {
  registrationForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService
  ) {
    this.registrationForm = this.formBuilder.group({
      username: "",
      email: "",
      password: "",
      passwordConfirm: "",
    });
  }
  onSubmit(userData) {
    // this.registrationForm.reset();
    this.userService
      .register(userData)
      .subscribe((result) => console.log('register successful', result));
  }
}

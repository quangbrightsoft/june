import { Component } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { UserService } from "../../user.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "login.component.html",
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService
  ) {
    this.loginForm = this.formBuilder.group({
      id: "",
      password: "",
    });
  }
  onLogin() {
    this.userService.login(this.loginForm.value).subscribe((result) => {
      if (!result.errors) {
        document.cookie = "token=" + result.data.login.accessToken;
      } else {
        console.error("ERROR in login", result.errors);
      }
    });
  }
}

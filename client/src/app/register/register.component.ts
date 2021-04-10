import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  @Output() cancelRegister = new EventEmitter();

  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];

  ngOnInit() {
    this.initForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
    }, error => {
      this.validationErrors = error;
    })
  }

  initForm() {
    this.registerForm = new FormGroup({
      gender: new FormControl('male', Validators.required),
      username: new FormControl('', Validators.required),
      knownAs: new FormControl('', Validators.required),
      dateOfBirth: new FormControl(null, Validators.required),
      city: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')])
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isMatching : true};
    }

  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}

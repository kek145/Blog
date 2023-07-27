import { Component } from '@angular/core';
import { IRegistrationDto } from 'src/app/models/registrationDto';
import { RegistrationService } from 'src/app/services/registration.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  selectedValueUser: string = 'User';
  selectedValueAuthor: string = 'Author';
  registrationDto: IRegistrationDto = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: ''
  };

  constructor(private registration: RegistrationService) {}

  registrationUser(user: IRegistrationDto): void {
    this.registration.registration(user).subscribe(
      response => {
        Swal.fire('Successfully', 'Account created successfully!', 'success');
      },
      error => {
        Swal.fire('Error', `${error.error}`, 'error');
      }
    );
  }
}

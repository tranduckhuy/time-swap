import { Component, input } from '@angular/core';
import { ApplicantModel } from '../../../../../../shared/models/entities/applicant.model';

@Component({
  selector: 'div[app-applicant-card]',
  standalone: true,
  imports: [],
  templateUrl: './applicant-card.component.html',
  styleUrl: './applicant-card.component.css'
})
export class ApplicantCardComponent {
  applicant = input.required<ApplicantModel>()
}

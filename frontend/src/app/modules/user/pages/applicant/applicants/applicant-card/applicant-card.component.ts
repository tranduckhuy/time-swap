import { Component, inject, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { ApplicantModel } from '../../../../../../shared/models/entities/applicant.model';

@Component({
  selector: 'div[app-applicant-card]',
  standalone: true,
  imports: [DatePipe, RouterLink],
  templateUrl: './applicant-card.component.html',
  styleUrl: './applicant-card.component.css',
})
export class ApplicantCardComponent {
  applicant = input.required<ApplicantModel>();
}

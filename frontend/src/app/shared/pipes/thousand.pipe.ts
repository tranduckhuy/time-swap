import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'thousand',
  standalone: true
})
export class ThousandPipe implements PipeTransform {

  transform(value: number | string): string {
    if (!value) return '';
    return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  }

}

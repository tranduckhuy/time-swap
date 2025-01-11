import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'thousand',
  standalone: true
})
export class ThousandPipe implements PipeTransform {

  transform(value: number | string): string {
    if (!value) return '';
    return value.toString().replace(/(?<=\d)(?=(?:\d{3})+$)/g, ',');
  }
}

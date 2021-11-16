import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../utils/constants';

@Pipe({
  name: 'datetimeFormat',
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {
  transform(value: any): any {
    const [date, time] = String(value).split(' ');
    const [day, month, year] = date.split('/');
    const [hour, minute, seconds] = time.split(':');
    const dateString = `${month}/${day}/${year} ${hour}:${minute}:${seconds}`;
    return super.transform(dateString, Constants.DATE_TIME_FMT);
  }
}

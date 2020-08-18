import { Pipe, PipeTransform, Injectable } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Constantes } from '../Util/Constantes';

@Pipe({
  name: 'DateTimeFormatPipe'
})
@Injectable()
export class DateTimeFormatPipePipe extends DatePipe implements PipeTransform {
  transform(value: any, args?: any): any {
    return super.transform(value, Constantes.DATE_TIME_FMT);
  }
}

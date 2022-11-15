import { Pipe, PipeTransform } from '@angular/core';
import { TextTransformService } from '../services/text-transform.service';

@Pipe({
  name: 'capitalizeFirst',
})
export class CapitalizeFirstPipe implements PipeTransform {
  constructor(private readonly textTransform: TextTransformService) {}

  transform(text: string): string {
    return this.textTransform.capitalizeFirst(text);
  }
}

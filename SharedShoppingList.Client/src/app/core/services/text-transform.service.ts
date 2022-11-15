import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TextTransformService {
  capitalizeFirst(text: string): string {
    return `${text[0].toUpperCase()}${text.slice(1)}`;
  }
}

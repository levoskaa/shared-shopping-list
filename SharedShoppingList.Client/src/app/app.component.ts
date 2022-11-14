import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  constructor(
    private readonly primengConfig: PrimeNGConfig,
    private readonly translate: TranslateService
  ) {}

  ngOnInit() {
    this.primengConfig.ripple = true;
    // The lang to use.
    // If the lang isn't available, it will use the current loader to get them.
    this.translate.use('en');
  }
}

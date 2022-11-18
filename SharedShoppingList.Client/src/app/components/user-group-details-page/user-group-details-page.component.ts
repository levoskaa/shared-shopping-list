import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { isInt } from 'src/app/core/utils/isInt';

@Component({
  templateUrl: './user-group-details-page.component.html',
  styleUrls: ['./user-group-details-page.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserGroupDetailsPageComponent implements OnInit {
  groupId!: number;

  constructor(private route: ActivatedRoute, private readonly router: Router) {}

  ngOnInit(): void {
    this.initData();
  }

  private initData(): void {
    const groupId = this.route.snapshot.paramMap.get('id');
    if (groupId !== null && !isInt(groupId)) {
      this.router.navigateByUrl('/');
    }
    this.groupId = parseInt(groupId!);
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  member: Member;
  gallaryImages: any;
  myInterval = 0;
  activeSlideIndex = 0;

  constructor(
    private memberService: MembersService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.loadMember();
  }

  getImages() {
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        image: photo?.url
      });
    }
    return imageUrls;
  }

  loadMember() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('username'))
      .subscribe(m => {
        this.member = m;
        this.gallaryImages = this.getImages();
      });
  }

  selectTab(id: number) {

  }

  sendLike(id: number) {

  }

}

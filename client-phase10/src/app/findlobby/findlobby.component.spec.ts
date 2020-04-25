import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FindlobbyComponent } from './findlobby.component';

describe('FindlobbyComponent', () => {
  let component: FindlobbyComponent;
  let fixture: ComponentFixture<FindlobbyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FindlobbyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FindlobbyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

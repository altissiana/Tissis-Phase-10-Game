import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WildCardRunComponent } from './wild-card-run.component';

describe('WildCardRunComponent', () => {
  let component: WildCardRunComponent;
  let fixture: ComponentFixture<WildCardRunComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WildCardRunComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WildCardRunComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

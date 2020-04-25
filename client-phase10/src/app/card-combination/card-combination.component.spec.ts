import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardCombinationComponent } from './card-combination.component';

describe('CardCombinationComponent', () => {
  let component: CardCombinationComponent;
  let fixture: ComponentFixture<CardCombinationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardCombinationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardCombinationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

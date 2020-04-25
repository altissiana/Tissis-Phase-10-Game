import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TissiComponent } from './tissi.component';

describe('TissiComponent', () => {
  let component: TissiComponent;
  let fixture: ComponentFixture<TissiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TissiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TissiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

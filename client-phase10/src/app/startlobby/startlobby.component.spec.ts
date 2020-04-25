import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StartlobbyComponent } from './startlobby.component';

describe('StartlobbyComponent', () => {
  let component: StartlobbyComponent;
  let fixture: ComponentFixture<StartlobbyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StartlobbyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StartlobbyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

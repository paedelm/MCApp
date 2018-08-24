/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MutationsComponent } from './mutations.component';

describe('MutationsComponent', () => {
  let component: MutationsComponent;
  let fixture: ComponentFixture<MutationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MutationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MutationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

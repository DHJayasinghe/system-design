import ***REMOVED*** TestBed ***REMOVED*** from '@angular/core/testing';
import ***REMOVED*** AppComponent ***REMOVED*** from './***REMOVED***.component';

describe('AppComponent', () => ***REMOVED***
  beforeEach(async () => ***REMOVED***
    await TestBed.configureTestingModule(***REMOVED***
      declarations: [
        AppComponent
      ],
***REMOVED***).compileComponents();
  ***REMOVED***);

  it('should create the ***REMOVED***', () => ***REMOVED***
    const fixture = TestBed.createComponent(AppComponent);
    const ***REMOVED*** = fixture.componentInstance;
    expect(***REMOVED***).toBeTruthy();
  ***REMOVED***);

  it(`should have as title 'facebook-spa'`, () => ***REMOVED***
    const fixture = TestBed.createComponent(AppComponent);
    const ***REMOVED*** = fixture.componentInstance;
    expect(***REMOVED***.title).toEqual('facebook-spa');
  ***REMOVED***);

  it('should render title', () => ***REMOVED***
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.content span')?.textContent).toContain('facebook-spa ***REMOVED*** is running!');
  ***REMOVED***);
***REMOVED***);

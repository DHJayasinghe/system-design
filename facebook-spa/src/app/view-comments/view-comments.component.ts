import ***REMOVED*** HttpClient ***REMOVED*** from '@angular/common/http';
import ***REMOVED*** Component, EventEmitter, Input, OnInit, Output ***REMOVED*** from '@angular/core';
import ***REMOVED*** FormBuilder, FormControl, FormGroup, Validators ***REMOVED*** from '@angular/forms';
import ***REMOVED*** environment ***REMOVED*** from 'src/environments/environment';

@Component(***REMOVED***
  selector: '***REMOVED***-view-comments',
  templateUrl: './view-comments.component.html',
  styleUrls: ['./view-comments.component.css']
***REMOVED***)
export class ViewCommentsComponent implements OnInit ***REMOVED***
  @Input() postId?: string;
  @Output() dismissed = new EventEmitter<boolean>(false);

  placeCommentForm = new FormGroup(***REMOVED***
    content: new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(5000)
    ])
  ***REMOVED***);

  constructor(private http: HttpClient, private formBuilder: FormBuilder) ***REMOVED*** ***REMOVED***

  ngOnInit() ***REMOVED***
  ***REMOVED***

  addComment() ***REMOVED***
    if (!this.placeCommentForm.valid) return;
    this.http.post(`$***REMOVED***environment.baseUrl***REMOVED***/posts/$***REMOVED***this.postId***REMOVED***/comments`, this.placeCommentForm.value)
      .subscribe(***REMOVED***
        next: () => ***REMOVED***
          this.placeCommentForm.reset();
    ***REMOVED***,
        error: (err) => ***REMOVED***
          console.error(err);
    ***REMOVED***
  ***REMOVED***);
  ***REMOVED***

  dismiss() ***REMOVED***
    this.dismissed.emit(true);
  ***REMOVED***
***REMOVED***

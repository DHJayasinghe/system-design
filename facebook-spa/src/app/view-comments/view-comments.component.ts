import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-view-comments',
  templateUrl: './view-comments.component.html',
  styleUrls: ['./view-comments.component.css']
})
export class ViewCommentsComponent implements OnInit {
  @Input() postId?: string;
  @Output() dismissed = new EventEmitter<boolean>(false);

  placeCommentForm = new FormGroup({
    content: new FormControl('', [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(5000)
    ])
  });

  constructor(private http: HttpClient, private formBuilder: FormBuilder) { }

  ngOnInit() {
  }

  addComment() {
    if (!this.placeCommentForm.valid) return;
    this.http.post(`${environment.baseUrl}/posts/${this.postId}/comments`, this.placeCommentForm.value)
      .subscribe({
        next: () => {
          this.placeCommentForm.reset();
        },
        error: (err) => {
          console.error(err);
        }
      });
  }

  dismiss() {
    this.dismissed.emit(true);
  }
}

/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { FileUploadComponent } from './file-upload.component';

let component: FileUploadComponent;
let fixture: ComponentFixture<FileUploadComponent>;

describe('file-upload component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ FileUploadComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(FileUploadComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
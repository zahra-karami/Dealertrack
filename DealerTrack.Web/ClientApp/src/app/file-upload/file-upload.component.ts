import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})


export class FileUploadComponent implements OnInit {

  public messages = [];
  public sales: VehicleSales[];
  public filename = '';
  public mostOftenSoldVehicle = '';


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  public uploader: FileUploader = new FileUploader({
    url: this.baseUrl + 'VehicleSale/UploadFile',
    maxFileSize: 1 * 1024 * 1024, // 1 MB,
    allowedMimeType: ['application/vnd.ms-excel'],

  });

  ngOnInit() {

    this.uploader.onCompleteItem = (item: any, res: any, status: any) => {

      if (status === 200) {
        var response = JSON.parse(res);
        if (response.isSucceeded) {
          this.sales = response.result.list;
          this.mostOftenSoldVehicle = response.result.mostOftenSoldVehicle;
          this.filename = item.file.name;
          this.messages = [];
        } else {
          this.messages = response.responseMessage;
        }
      } else {
        this.messages.push('An Internal error occurred at server side');
      }
    };

    this.uploader.onWhenAddingFileFailed = (item: any, filter: any, options: any) => {

      this.messages = [];

      if (item.size > options.maxFileSize) {
        var size = options.maxFileSize / 1024 / 1024;
        this.messages.push('The maximum supported file size is ' + size + 'MB');
      }

      if (item.type > options.allowedMimeType[0]) {
        this.messages.push('Invalid file type!');
      }
    }
  }



  public hasBaseDropZoneOver: boolean = false;
  public hasAnotherDropZoneOver: boolean = false;

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  public getSales() {
    this.http.get<VehicleSales[]>(this.baseUrl + 'VehicleSale/Get').subscribe(result => {
      this.sales = result;
    }, error => console.error(error));
  }
}

interface VehicleSales {
  dealNumber: number;
  customerName: string;
  dealershipName: string;
  vehicle: string;
  price: number;
  date: Date;
}


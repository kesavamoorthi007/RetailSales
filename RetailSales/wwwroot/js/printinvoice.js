function printInvoice() {
        const invoiceContents = document.querySelector('.invoice').innerHTML;
        const printWindow = window.open('', '_blank', 'height=800,width=600');

        if (!printWindow) {
            alert('Popup blocked! Please allow popups for this site.');
            return;
        }

        printWindow.document.write(`
            <html>
                <head>
                    <title>Print Invoice</title>
                    <style>
                        @media print {
                            body {
                                margin: 0;
                                padding: 10mm;
                                font-size: 12px;
                            }
                            .invoice {
                                width: 190mm;
                                margin: 0 auto;
                            }
                            table {
                                width: 100%;
                                border-collapse: collapse;
                            }
                            th, td {
                                border: 1px solid #000;
                                padding: 4px;
                            }
                            hr {
                                margin: 5px 0;
                            }
                            .row {
                                display: flex;
                                flex-wrap: wrap;
                                margin-bottom: 5px;
                            }
                            .col-md-6, .col-md-4, .col-md-8, .col-md-3, .col-md-9 {
                                flex: 1;
                                max-width: 50%;
                                box-sizing: border-box;
                            }
                            .col-md-4 { max-width: 33.33%; }
                            .col-md-8 { max-width: 66.66%; }
                            .col-md-3 { max-width: 25%; }
                            .col-md-9 { max-width: 75%; }
                            .text-md-end { text-align: right; }
                            .text-md-start { text-align: left; }
                            img {
                                max-width: 250px;
                                height: auto;
                            }
                            p, h4, h5 {
                                margin: 2px 0;
                            }
                        }
                    </style>
                </head>
                <body>
                    <div class="invoice">
                        ${invoiceContents}
                    </div>
                    <script>
                        window.onload = function() {
                            window.print();
                            window.onafterprint = function() {
                                window.close();
                            };
                        };
                    <\/script>
                </body>
            </html>
        `);

        printWindow.document.close();
    }
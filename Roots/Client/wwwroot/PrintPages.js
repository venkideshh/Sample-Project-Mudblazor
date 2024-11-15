function PrintPage() {
    // Open a new window for printing
    var printWindow = window.open('', '_blank');

    // Get the HTML content of the MudBlazor screen
    var content = document.documentElement.outerHTML;

    // Replace the print button with an empty string in the content
    content = content.replace('<mud-button variant="variant.outlined" color="color.primary" starticon="@icons.material.filled.print" class="mt-7 printbutton" onclick="PrintPage()">Print</mud-button>', '');

    // Add CSS styles to the content to define the border for the printed page and set default scale to 85%
    var css = '<style type="text/css">' +
        '@media print {' +
        'body {' +
        //'border: 1px solid #000; /* Set border color and width */' +
        //'padding: 20px; /* Add padding to separate content from border */' +
        //'-webkit-print-color-adjust: exact; /* Enable background graphics */' +
        'background-color: white;' +
        '}' +
        '@page {' +
        '    size: auto;   /* auto is the initial value */' +
        '    margin: 5mm 5mm 5mm 5mm; /* margin is on all sides */' +
        '    -webkit-transform: scale(0.5); /* Set default scale to 85% for webkit browsers */' +
        '    transform: scale(0.8); /* Set default scale to 80% */' +
        '}' +
        '}' +
        '</style>';

    // Append the CSS styles to the content
    content = css + content;

    // Write the content to the new window
    printWindow.document.open();
    printWindow.document.write(content);
    printWindow.document.close();

    // Wait for the window to load before calling the print function
    printWindow.onload = function () {
        printWindow.print();
        printWindow.onafterprint = function () {
            // Close the print window after printing
            printWindow.close();
        };
    };
}

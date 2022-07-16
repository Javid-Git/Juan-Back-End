
$(document).ready(function () {
    $(document).on('click', '.addinputs', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.inputsContainer').append(data)
            })
    })
    $(document).on('click', '.deleteBtn', function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(res => res.text())
                    .then(data => { $('.tblContent').html(data) })
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
       
    })
    $(document).on('click', '.restoreBtn', function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, restore it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(res => {
                        if (res.status == 400) {
                            Swal.fire(
                                'Not restored',
                                'Main category is not existing',
                                'error'
                            )
                        } else {
                            return res.text()
                        }
                    })
                        
                    .then(data => { $('.tblContent').html(data) })
                Swal.fire(
                    'Restored!',
                    'Your file has been restored.',
                    'success'
                )
            }
        })

    })

    //$(document).on('change', '.ismainbtn', function () {
    //    console.log($(this).is(":checked"))
    //    if ($(this).is(":checked")) {
    //        $('.imgfield').removeClass('d-none')
    //        $('.parentfield').addClass('d-none')
    //    }
    //    else {
    //        $('.imgfield').addClass('d-none')
    //        $('.parentfield').removeClass('d-none')
    //    }
    //})
    //if ($('.ismainbtn').is(":checked")) {
    //    $('.imgfield').removeClass('d-none')
    //    $('.parentfield').addClass('d-none')
    //}
    //else {
    //    $('.imgfield').addClass('d-none')
    //    $('.parentfield').removeClass('d-none')
    //}
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    if ($('#successInput').val().length) {
        toastr["success"]($('#successInput').val(), $('#successInput').val().split(' ')[0])
    }
})
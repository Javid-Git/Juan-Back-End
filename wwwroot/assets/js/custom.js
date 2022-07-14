$(document).ready(function () {
    $(document).on('click', '.modaldetail', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');

        console.log(url);

        fetch(url).then(response => {
            return response.text();
        }).then(data => {
            $('.modal-content').html(data);
            $('.quick-view-image').slick({
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: false,
                dots: false,
                fade: true,
                asNavFor: '.quick-view-thumb',
                speed: 400,
            });

            $('.quick-view-thumb').slick({
                slidesToShow: 4,
                slidesToScroll: 1,
                asNavFor: '.quick-view-image',
                dots: false,
                arrows: false,
                focusOnSelect: true,
                speed: 400,
            });
            
        })
    })
    $(document).on('click', '.sortlink', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.sort-content').html(data);
            });
    })
    //$('.sortlink').click(function (e) {
    //    e.preventDefault();
    //    let url = $(this).attr('href');

    //    console.log(url);

    //    fetch(url).then(response => {
    //        return response.text();
    //    }).then(data => {
    //        $('.sort-content').html(data);
    //    })
    //})
    $('.searchinput').keyup(function () {
        console.log("eee");
        let value = $(this).val();
        console.log(value);

        let url = $(this).data("url");
        url = url + '?search=' + value.trim();
        console.log(url);

        if (value) {
            fetch(url)
                .then(response => response.text())

                .then(data => {
                    $('.search-body .list-group').html(data)

                })
        }
        else {
            $('.search-body .list-group').html('')
        }

    })
   
    $(document).on('click', '.addtobasket', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        console.log(url)
        fetch(url)
            .then(res => res.json())
            .then(data => {
                $('.notification').html(data);
            });
    })
    $(document).on('click', '.openbasket', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        console.log(url)
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.header-cart').html(data);
            });
    })

})

//$(document).ready(function () {
//    $('.delete-form').on('submit', function (e) {
//        e.preventDefault(); // Ngăn form submit ngay lập tức
//        var form = $(this);
//        var productName = form.find('.delete-btn').data('name');

//        Swal.fire({
//            title: "Bạn có chắc chắn không?",
//            text: "Bạn sẽ không thể khôi phục sản phẩm " + productName + " sau khi xóa!",
//            icon: "warning",
//            showCancelButton: true,
//            confirmButtonText: "Có, xóa nó!",
//            cancelButtonText: "Không, hủy bỏ!",
//            confirmButtonColor: "#28a745",
//            cancelButtonColor: "#dc3545"
//        }).then((result) => {
//            if (result.isConfirmed) {
//                form.off('submit'); // Tắt sự kiện để tránh vòng lặp
//                form.submit(); // Gửi form đến DeleteConfirmed
//            }
//        });
//    });
//});
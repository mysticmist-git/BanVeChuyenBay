-- Nhập chuyến bay
USE BANVECHUYENBAY2

INSERT INTO SANBAY (TenSanBay, VietTat, TinhThanh, TrangThai) VALUES
	(N'Nội Bài', 'HAN', N'Hà Nội', 1),
	(N'Cát Bi', 'HPH', N'Hải Phòng', 1),
	(N'Điện Biên Phủ', 'DIN', N'Điện Biên', 1),
	(N'Thọ Xuân', 'THD', N'Thanh Hóa', 1),
	(N'Vinh', N'VII', N'Nghệ An', 1),
	(N'Đồng Hới', 'VDH', N' Quảng Bình', 1),
	(N'Phú Bài', 'HUI', N'Thừa Thiên - Huế', 1),
	(N'Đà Nẵng', 'DAD', N'Đà Nẵng', 1),
	(N'Chu Lai', 'VCL', N'Quảng Nam', 1),
	(N'Phù Cát', 'UIH', N'Bình Định', 1),
	(N'Tuy Hòa', 'TBB', N'Phú Yên', 1),
	(N'Cam Ranh', 'CXR', N'Khánh Hòa', 1),
	(N'Buôn Ma Thuột', 'BMV', N'Đắk Lắk', 1),
	(N'Liên Khương', 'DLI', N'Lâm Đồng', 1),
	(N'Pleiku', 'PXU', N'Gia Lai', 1),
	(N'Tân Sơn Nhất', 'SGN', N'TP HCM', 1),
	(N'Cà Mau', 'CAH', N'Cà Mau', 1),
	(N'Côn Đảo', 'VCS', N'Bà Rịa-Vũng Tàu', 1),
	(N'Cần Thơ', 'VCA', N'Cần Thơ', 1),
	(N'Rạch Giá', 'VKG', N'Kiên Giang', 1),
	(N'Phú Quốc', 'PQC', N'Kiên Giang', 1),
	(N'Vân Đồn', 'VDO', N'Quảng Ninh', 1)

-- Nhập Đường bay
INSERT INTO DUONGBAY (MaSanBayDen, MaSanBayDi, ThoiGianBay) VALUES (16, 1, 180)
INSERT INTO DUONGBAY (MaSanBayDen, MaSanBayDi, ThoiGianBay) VALUES (1, 16, 200)
INSERT INTO DUONGBAY (MaSanBayDen, MaSanBayDi, ThoiGianBay) VALUES (1, 14, 160)

-- Nhập sân bay trung gian
INSERT INTO SANBAYTG(MaDuongBay, MaSanBay, ThuTu, ThoiGianDung, GhiChu) VALUES (1, 5, 1, 13, N'Dừng cho vui')
INSERT INTO SANBAYTG(MaDuongBay, MaSanBay, ThuTu, ThoiGianDung, GhiChu) VALUES (1, 7, 2, 14, N'Dừng cho vui x3')

INSERT INTO SANBAYTG(MaDuongBay, MaSanBay, ThuTu, ThoiGianDung, GhiChu) VALUES (2, 8, 1, 19, N'Load hàng nóng')

INSERT INTO SANBAYTG(MaDuongBay, MaSanBay, ThuTu, ThoiGianDung, GhiChu) VALUES (3, 10, 1, 12, N'Cho khách đi WC?')
INSERT INTO SANBAYTG(MaDuongBay, MaSanBay, ThuTu, ThoiGianDung, GhiChu) VALUES (3, 11, 2, 13, N'Cho khách mua đồ lưu niệm?')

-- Nhập chuyến bay
INSERT INTO CHUYENBAY (MaDuongBay, GiaVe, NgayGio) VALUES (1, 500000, CONVERT(DATETIME,'14/4/2022',103))
INSERT INTO CHUYENBAY (MaDuongBay, GiaVe, NgayGio) VALUES (2, 620000, CONVERT(DATETIME,'15/4/2022',103))
INSERT INTO CHUYENBAY (MaDuongBay, GiaVe, NgayGio) VALUES (3, 770000, CONVERT(DATETIME,'16/4/2022',103))

-- Nhập hạng vé
INSERT INTO HANGVE (TenHangVe, HeSo, TrangThai) VALUES (N'Hạng Nhất', 1.6, 1)
INSERT INTO HANGVE (TenHangVe, HeSo, TrangThai) VALUES (N'Thương gia', 1.4, 1)
INSERT INTO HANGVE (TenHangVe, HeSo, TrangThai) VALUES (N'Phổ thông', 1.2, 1)

-- Nhập chi tiết hạng vé

INSERT INTO CHITIETHANGVE (MaChuyenBay, MaHangVe, SoGhe) VALUES (1, 1, 10)
INSERT INTO CHITIETHANGVE (MaChuyenBay, MaHangVe, SoGhe) VALUES (1, 2, 20)
INSERT INTO CHITIETHANGVE (MaChuyenBay, MaHangVe, SoGhe) VALUES (1, 3, 60)

INSERT INTO CHITIETHANGVE (MaChuyenBay, MaHangVe, SoGhe) VALUES (2, 1, 20)
INSERT INTO CHITIETHANGVE (MaChuyenBay, MaHangVe, SoGhe) VALUES (2, 2, 40)

INSERT INTO CHITIETHANGVE (MaChuyenBay, MaHangVe, SoGhe) VALUES (3, 2, 20)
INSERT INTO CHITIETHANGVE (MaChuyenBay, MaHangVe, SoGhe) VALUES (3, 3, 90)

-- Nhập Khách hàng
INSERT INTO KHACHHANG (HoTen, CMND, SDT, Email) VALUES (N'Nguyễn Văn Hên', '123456789', '0000000001', 'hen@mail.com')
INSERT INTO KHACHHANG (HoTen, CMND, SDT, Email) VALUES (N'Phan Trường Huy', '135792460', '0000000002', 'huy@mail.com')
INSERT INTO KHACHHANG (HoTen, CMND, SDT, Email) VALUES (N'Hàn Phi Trường', '2468135790', '0000000003', 'truong@mail.com')
INSERT INTO KHACHHANG (HoTen, CMND, SDT, Email) VALUES (N'Trần Nguyễn Nhật Tân', '111222333', '0000000004', 'tan@mail.com')
INSERT INTO KHACHHANG (HoTen, CMND, SDT, Email) VALUES (N'Nguyễn Đình Thi', '444555666', '0000000005', 'thi@mail.com')

-- Nhập vé
INSERT INTO VE (MaKhachHang, MaHangVe, MaChuyenBay, NgayThanhToan) VALUES (1, 1, 1, convert(datetime, '15/4/2022', 103))
INSERT INTO VE (MaKhachHang, MaHangVe, MaChuyenBay, NgayThanhToan) VALUES (2, 2, 2, convert(datetime, '16/4/2022', 103))

-- Nhập đặt chỗ
INSERT INTO DATCHO (MaNguoiDat, MaHangVe, MaChuyenBay, SoVeDat) VALUES (3, 3, 3, 3)

-- Nhập chi tiết đặt chỗ
INSERT INTO CHITIETDATCHO (MaDatCho, MaKhachHang) VALUES (1, 3)
INSERT INTO CHITIETDATCHO (MaDatCho, MaKhachHang) VALUES (1, 4)
INSERT INTO CHITIETDATCHO (MaDatCho, MaKhachHang) VALUES (1, 5)
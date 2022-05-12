
CREATE DATABASE BANVECHUYENBAY
GO

USE BANVECHUYENBAY
GO

-- Bảng Chuyến bay
CREATE TABLE CHUYENBAY
(
	MaChuyenBay INT IDENTITY(1,1) PRIMARY KEY,
	MaDuongBay INT NOT NULL,
	GiaVe MONEY NOT NULL,
	NgayGio DATETIME NOT NULL,
	DaKhoiHanh BIT NOT NULL
)
GO

-- Bảng Sân bay
CREATE TABLE SANBAY
(
	MaSanBay INT IDENTITY(1,1) PRIMARY KEY,
	TenSanBay NVARCHAR(200) NOT NULL,
	VietTat VARCHAR(50) NOT NULL,
	TinhThanh NVARCHAR(200) NOT NULL,
	TrangThai BIT NOT NULL,
)
GO

-- Bảng Sân bay trung gian
CREATE TABLE SANBAYTG
(
	MaSanBayTG INT IDENTITY(1,1) PRIMARY KEY,
	MaDuongBay INT NOT NULL,
	MaSanBay INT NOT NULL,
	ThuTu INT NOT NULL,
	ThoiGianDung INT NOT NULL,
	GhiChu NVARCHAR(200)	
)
GO

-- Bảng Đường bay
CREATE TABLE DUONGBAY
(
	MaDuongBay INT IDENTITY(1,1) PRIMARY KEY,
	MaSanBayDi INT NOT NULL,
	MaSanBayDen INT NOT NULL,
	ThoiGianBay INT NOT NULL
)
GO

-- Bảng Hạng vé
CREATE TABLE HANGVE
(
	MaHangVe INT IDENTITY(1,1) PRIMARY KEY,
	TenHangVe NVARCHAR(200) NOT NULL,
	HeSo DECIMAL(3,2) NOT NULL,
	TrangThai BIT NOT NULL
)
GO

-- Bảng Chi tiết hạng vé
CREATE TABLE CHITIETHANGVE
(
	MaCTHV INT IDENTITY(1,1) PRIMARY KEY,
	MaHangVe INT NOT NULL,
	MaChuyenBay INT NOT NULL,
	SoGhe INT NOT NULL,
)
GO

-- Bảng vé
CREATE TABLE VE
(
	MaVe INT IDENTITY(1,1) PRIMARY KEY,
	MaHangVe INT NOT NULL,
	MaChuyenBay INT NOT NULL,
	MaKhachHang INT NOT NULL,
	GiaTien MONEY NOT NULL DEFAULT 0,
	NgayThanhToan DATETIME NOT NULL
)
GO

-- Bảng Khách hàng
CREATE TABLE KHACHHANG
(
	MaKhachHang INT IDENTITY(1,1) PRIMARY KEY,
	HoTen NVARCHAR(200) NOT NULL,
	CMND VARCHAR(100) NOT NULL,
	SDT VARCHAR(50),
	Email VARCHAR(max)
)
GO

-- Bảng Đặt chỗ
CREATE TABLE DATCHO
(
	MaDatCho INT IDENTITY(1,1) PRIMARY KEY,
	MaHangVe INT NOT NULL,
	MaChuyenBay INT NOT NULL,
	MaNguoiDat INT NOT NULL,
	NgayGioDat DATETIME NOT NULL DEFAULT GETDATE(),
	SoVeDat INT NOT NULL,
	GiaTien MONEY NOT NULL DEFAULT 0,
	TrangThai VARCHAR(20)
)
GO

-- Bảng Chi tiết đặt chỗ
CREATE TABLE CHITIETDATCHO
(
	MaChiTietDatCho INT IDENTITY(1,1) PRIMARY KEY,
	MaDatCho INT NOT NULL,
	MaKhachHang INT NOT NULL
)
GO

-- Bảng Doanh thu chuyến bay
CREATE TABLE DOANHTHUCHUYENBAY
(
	MaDoanhThuChuyenBay INT IDENTITY(1,1) PRIMARY KEY,
	MaDoanhThuThang INT NOT NULL,
	MaChuyenBay INT NOT NULL,
	SoVe INT NOT NULL,
	DoanhThu MONEY NOT NULL,
	TiLe DECIMAL(3,2) NOT NULL
)
GO

-- Bảng Doanh thu tháng
CREATE TABLE DOANHTHUTHANG
(
	MaDoanhThuThang INT IDENTITY(1,1) PRIMARY KEY,
	MaDoanhThuNam INT NOT NULL,
	Thang INT NOT NULL,
	SoChuyenBay INT NOT NULL,
	DoanhThu MONEY NOT NULL,
	TiLe DECIMAL(5,2) NOT NULL
)
GO

-- Bảng Doanh thu năm
CREATE TABLE DOANHTHUNAM
(
	MaDoanhThuNam INT IDENTITY(1,1) PRIMARY KEY,
	Nam INT NOT NULL,
	SoChuyenBay INT NOT NULL,
	DoanhThu MONEY NOT NULL
)
GO

-- Bảng Tham số
CREATE TABLE THAMSO
(
	TenThamSo VARCHAR(100) PRIMARY KEY,
	GiaTri INT NOT NULL
)
GO

--========================================= RÀNG BUỘC BẢNG CHUYẾN BAY =========================================
------ Khóa ngoại Mã đường bay
ALTER TABLE CHUYENBAY ADD CONSTRAINT FK_CHUYENBAY_MaDuongBay FOREIGN KEY (MaDuongBay) REFERENCES DUONGBAY(MaDuongBay)
GO

------ GiaVe: Số dương
ALTER TABLE CHUYENBAY ADD CONSTRAINT CK_CHUYENBAY_GiaVe CHECK (GiaVe>=0)
GO

------ NgayGio: lớn hơn ngày giờ tại thời điểm nhận lịch chuyến bay (DISABLE ĐỂ DỄ TEST DỮ LIỆU)
--ALTER TABLE CHUYENBAY ADD CONSTRAINT CK_CHUYENBAY_NgayGio CHECK (NgayGio>GETDATE())
--GO

------ DaKhoiHanh: mặc đinh là 0 (chưa khởi hành)
ALTER TABLE CHUYENBAY ADD CONSTRAINT DF_CHUYENBAY_DaKhoiHanh DEFAULT 0 FOR DaKhoiHanh
GO

-- RÀNG BUỘC BẢNG SÂN BAY
------ VietTat Unique
ALTER TABLE SANBAY ADD CONSTRAINT UNIQUE_SANBAY_VietTat UNIQUE (VietTat)
GO

--========================================= RÀNG BUỘC BẢNG SÂN BAY TRUNG GIAN =========================================
------ Khóa ngoại Mã đường bay
ALTER TABLE SANBAYTG ADD CONSTRAINT FK_SANBAYTG_MaDuongBay FOREIGN KEY (MaDuongBay) REFERENCES DUONGBAY(MaDuongBay)
GO
------ Khóa ngoại Mã sân bay
ALTER TABLE SANBAYTG ADD CONSTRAINT FK_SANBAYTG_MaSanBay FOREIGN KEY (MaSanBay) REFERENCES SANBAY(MaSanBay)
GO
------ ThuTu: >= 1
ALTER TABLE SANBAYTG ADD CONSTRAINT CK_SANBAYTG_ThuTu CHECK (ThuTu>=1)
GO

------ MaSanBay: không trùng trên cùng một đường bay
ALTER TABLE SANBAYTG ADD CONSTRAINT UNIQUE_SANBAYTG_MaSanBay UNIQUE(MaSanBay, MaDuongBay)
GO

------ TRIGGER MaSanBay: Không trùng với SanBayDi, SanBayDen của DUONGBAY
CREATE TRIGGER TG_SANBAYTG_MaSanBay ON SANBAYTG
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @maSanBayTG INT, @maSanBayDi INT, @maSanBayDen INT, @maDuongBay INT
	
	-- Lấy mã sân bay trung gian vừa thêm
	SELECT @maSanBayTG=MaSanBay, @maDuongBay=MaDuongBay
	FROM INSERTED

	-- Lấy mã sân bay đi và sân bay đến của đường bay
	SELECT @maSanBayDi=MaSanBayDi, @maSanBayDen=MaSanBayDen
	FROM DUONGBAY
	WHERE MaDuongBay=@maDuongBay

	-- Kiểm tra xem có trùng không
	IF (@maSanBayTG IN (@maSanBayDi, @maSanBayDen))
	BEGIN
		ROLLBACK TRAN
		PRINT 'San bay trung gian trung san bay di hoac den'
	END
END
GO

------ TRIGGER ThuTu: ThuTu của cùng một đường bay không trùng nhau
CREATE TRIGGER TG_SANBAYTG_ThuTu ON SANBAYTG
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @thuTu INT, @maDuongBay INT
	-- Lấy thứ tự sân bay trung gian được thêm
	SELECT @thuTu=ThuTu, @maDuongBay=MaDuongBay
	FROM INSERTED
	-- Nếu ThuTu của cùng một đường bay trùng nhau thì rollback tran
	IF ((
		SELECT COUNT(ThuTu)
		FROM SANBAYTG
		WHERE MaDuongBay=@maDuongBay AND ThuTu=@thuTu)>1)
			BEGIN
				ROLLBACK TRAN
				PRINT 'ThuTu san bay trung gian trung nhau'
			END
END
GO

------ TRIGGER ThoiGianDung: ThoiGianDung >= ThoiGianDungToiThieu && ThoiGianDung <= ThoiGianDungToiDa
CREATE TRIGGER TG_SANBAYTG_ThoiGianDung ON SANBAYTG
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @thoiGianDung INT
	-- Lấy thời gian dừng được thêm
	SELECT @thoiGianDung=ThoiGianDung
	FROM INSERTED

	-- Khai báo Thời gian dừng tối thiểu và thời gian dừng tối đa
	DECLARE @min INT, @max INT
	-- Lấy Thời gian dừng tối thiểu và thời gian dừng tối đa
	SELECT @min=GiaTri
	FROM THAMSO
	WHERE TenThamSo='ThoiGianDungToiThieu'

	SELECT @max=GiaTri
	FROM THAMSO
	WHERE TenThamSo='ThoiGianDungToiDa'

	-- Nếu Thời gian dừng không hợp lệ thì rollback tran
	IF (@thoiGianDung<@min OR @thoiGianDung>@max)
		BEGIN
			ROLLBACK TRAN
			PRINT 'ThoiGianBay khong hop le'
		END
END
GO

------ TRIGGER: So san bay TG không vượt SanBayTrungGianToiDa
CREATE TRIGGER TG_SANBAYTG_SanBayTrungGianToiDa ON SANBAYTG
FOR INSERT
AS
BEGIN
	DECLARE @sanBayTrungGianToiDa INT, @soSanBayTrungGianHienTai INT
	-- Lấy Sân bay trung gian tối đa
	SELECT @sanBayTrungGianToiDa=GiaTri
	FROM THAMSO
	WHERE TenThamSo='SoSanBayTrungGianToiDa'

	-- Lấy Số sân bay trung gian hiện tại
	SELECT @soSanBayTrungGianHienTai=COUNT(*)
	FROM SANBAYTG, INSERTED
	WHERE SANBAYTG.MaDuongBay=INSERTED.MaDuongBay

	-- Nếu số sân bay trung gian vượt thì rollback tran
	IF (@soSanBayTrungGianHienTai>@sanBayTrungGianToiDa)
		BEGIN
			ROLLBACK TRAN
			PRINT 'So san bay trung gian vuot qua So san bay trung gian toi da'
		END
END
GO

--========================================= RÀNG BUỘC ĐƯỜNG BAY =========================================
------ Khóa ngoại Mã sân bay đi
ALTER TABLE DUONGBAY ADD CONSTRAINT FK_DUONGBAY_MaSanBayDi FOREIGN KEY (MaSanBayDi) REFERENCES SANBAY(MaSanBay)
GO
------ Khóa ngoại Mã sân bay đến
ALTER TABLE DUONGBAY ADD CONSTRAINT FK_DUONGBAY_MaSanBayDen FOREIGN KEY (MaSanBayDen) REFERENCES SANBAY(MaSanBay)
GO
------ Trigger ThoiGianBay: ThoiGianBay >= ThoiGianBayToiThieu
CREATE TRIGGER TG_DUONGBAY_ThoiGianBay ON DUONGBAY
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @thoiGianBay INT
	-- Lấy Thời gian bay được thêm
	SELECT @thoiGianBay=ThoiGianBay
	FROM INSERTED

	-- Khai báo Thời gian bay tối tối thiểu
	DECLARE @min INT
	-- Lấy Thời gian dừng tối thiểu và thời gian dừng tối đa
	SELECT @min=GiaTri
	FROM THAMSO
	WHERE TenThamSo='ThoiGianBayToiThieu'
	-- Nếu Thời gian bay không hợp lệ thì rollback tran
	IF (@thoiGianBay<@min)
	BEGIN
		ROLLBACK TRAN
		PRINT 'Thoi gian bay be hon Thoi gian bay toi thieu'
	END
END
GO

----========================================= RÀNG BUỘC HẠNG VÉ --=========================================
------ HeSo: >= 0.0
ALTER TABLE HANGVE ADD CONSTRAINT CK_HANGVE_HeSo CHECK (HeSo>=0.0)
GO

--========================================= RÀNG BUỘC BẢNG CHI TIẾT HẠNG VÉ =========================================
------ Khóa ngoại Mã hạng vé
ALTER TABLE CHITIETHANGVE ADD CONSTRAINT FK_CHITIETHANGVE_MaHangVe FOREIGN KEY (MaHangVe) REFERENCES HANGVE(MaHangVe)
GO
------ Khóa ngoại Mã chuyến bay
ALTER TABLE CHITIETHANGVE ADD CONSTRAINT FK_CHITIETHANGVE_MaChuyenBay FOREIGN KEY (MaChuyenBay) REFERENCES CHUYENBAY(MaChuyenBay)
GO
------ SoGhe: lớn hơn 0
ALTER TABLE CHITIETHANGVE ADD CONSTRAINT CK_CHITIETHANGVE_SoGhe CHECK (SoGhe>0)
GO

------ MaHangVe: Unique cho mỗi chuyến bay
ALTER TABLE CHITIETHANGVE ADD CONSTRAINT UNIQUE_CHITIETHANGVE_MaHangVe UNIQUE (MaChuyenBay, MaHangVe)
GO

----========================================= RÀNG BUỘC BẢNG VÉ --=========================================
------ Khóa ngoại Mã hạng vé
ALTER TABLE VE ADD CONSTRAINT FK_VE_MaHangVe FOREIGN KEY (MaHangVe) REFERENCES HANGVE(MaHangVe)
GO
------ Khóa ngoại Mã chuyến bay
ALTER TABLE VE ADD CONSTRAINT FK_VE_MaChuyenBay FOREIGN KEY (MaChuyenBay) REFERENCES CHUYENBAY(MaChuyenBay)
GO
------ Khóa ngoại Mã khách hàng
ALTER TABLE VE ADD CONSTRAINT FK_VE_MaKhachHang FOREIGN KEY (MaKhachHang) REFERENCES KHACHHANG(MaKhachHang)
GO
------ GiaTien: không âm
ALTER TABLE VE ADD CONSTRAINT CK_VE_GiaTien CHECK (GiaTien>=0)
GO

------ NgayThanhToan: >= Ngày giờ hiện tại (DISABLE ĐỂ DỄ TEST)
--ALTER TABLE VE ADD CONSTRAINT CK_VE_NgayThanhToan CHECK (NgayThanhToan>=GETDATE())
--GO

------ GiaTien = CHUYENBAY.GiaVe*HANGVE.HeSo
CREATE TRIGGER TRG_VE_TinhGiaTien ON VE
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @maVe INT, @giaVe MONEY, @heSo DECIMAL(5,2)
	-- Lấy Mã vé, CHUYENBAY.GiaVe và HANGVE.HeSo
	SELECT @maVe=MaVe
	FROM INSERTED

	SELECT @giaVe=GiaVe
	FROM CHUYENBAY, INSERTED
	WHERE CHUYENBAY.MaChuyenBay=inserted.MaChuyenBay

	SELECT @heSo=HeSo
	FROM HANGVE, INSERTED
	WHERE HANGVE.MaHangVe=INSERTED.MaHangVe

	UPDATE VE
	SET GiaTien=(@giaVe*@heSo)
	WHERE VE.MaVe=@maVe
END
GO

--========================================= RÀNG BUỘC ĐẶT CHỖ =========================================
------ Khóa ngoại Mã hạng vé
ALTER TABLE DATCHO ADD CONSTRAINT FK_DATCHO_MaHangVe FOREIGN KEY (MaHangVe) REFERENCES HANGVE(MaHangVe)
GO
------ Khóa ngoại Mã chuyến bay
ALTER TABLE DATCHO ADD CONSTRAINT FK_DATCHO_MaChuyenBay FOREIGN KEY (MaChuyenBay) REFERENCES CHUYENBAY(MaChuyenBay)
GO
------ Khóa ngoại Mã người đặt
ALTER TABLE DATCHO ADD CONSTRAINT FK_DATCHO_MaNguoiDat FOREIGN KEY (MaNguoiDat) REFERENCES KHACHHANG(MaKhachHang)
GO
------ Trigger NgayGioDat: NgayGioDat <= (NgayKhoiHanh - ThoiGianDatVeChamNhat) (DISABLE ĐỂ DỄ TEST)
--CREATE TRIGGER TG_DATCHO_NgayGioDat ON DATCHO
--FOR INSERT, UPDATE
--AS
--BEGIN
--	-- Khai báo
--	DECLARE @ngayGioDat DATETIME
--	-- Lấy ngày giờ đăt chỗ được thêm
--	SELECT @ngayGioDat=NgayGioDat
--	FROM INSERTED

--	-- Lấy ngày giờ máy bay cất cánh
--	DECLARE @ngayGioCatCanh DATETIME

--	SELECT @ngayGioCatCanh=NgayGio
--	FROM CHUYENBAY
--	WHERE MaChuyenBay = (
--		SELECT MachuyenBay
--		FROM INSERTED
--	)

--	-- Khai báo ThoiGianDatVeChamNhat
--	DECLARE @datVeChamNhat INT

--	-- Lấy Thời gian dừng tối thiểu và thời gian dừng tối đa
--	SELECT @datVeChamNhat=GiaTri
--	FROM THAMSO
--	WHERE TenThamSo='ThoiGianDatVeChamNhat'

--	-- Nếu ngày giờ đặt chỗ không hợp lệ thì rollback tran
--	IF (@ngayGioDat>(DATEADD(day, -@datVeChamNhat, @ngayGioCatCanh)))
--	BEGIN
--		ROLLBACK TRAN
--		PRINT 'Ngay gio dat tre hon Ngay gio dat tre cham nhat'
--	END

--END
--GO

------ Trigger GiaTien: = HANGVE.HeSo * CHUYENBAY.GiaVe * SoVeDat
CREATE TRIGGER TG_DATCHO_NgayGioDat ON DATCHO
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @maDatCho INT, @maHangVe INT, @maChuyenBay INT, @heSo DECIMAL(3,2), @giaVe MONEY

	-- Lấy mã đặt chỗ
	SELECT @maDatCho=MaDatCho, @maHangVe=MaHangVe, @maChuyenBay = MaChuyenBay
	FROM INSERTED

	-- Lấy hệ số hạng vé
	SELECT @heSo=HeSo
	FROM HANGVE
	WHERE MaHangVe=@maHangVe

	-- Lấy giá vé
	SELECT @giaVe=GiaVe
	FROM CHUYENBAY
	WHERE MaChuyenBay=@maChuyenBay

	-- Update giá tiền
	UPDATE DATCHO
	SET GiaTien=SoVeDat * @giaVe * @heSo
	WHERE MaDatCho=@maDatCho
END
GO

------ TrangThai: ChuaDoi, DaDoi, DaHuy
ALTER TABLE DATCHO ADD CONSTRAINT CK_DATCHO_TrangThai CHECK (TrangThai IN ('ChuaDoi','DaDoi','DaHuy'))
GO	

------ TrangThai: Mặc định là chưa đổi
ALTER TABLE DATCHO ADD CONSTRAINT DF_DATCHO_TrangThai DEFAULT 'ChuaDoi' FOR TrangThai
GO

--========================================= RÀNG BUỘC BẢNG CHI TIẾT ĐẶT CHỖ =========================================
------ Khóa ngoại Mã đặt chỗ
ALTER TABLE CHITIETDATCHO ADD CONSTRAINT FK_CHITIETDATCHO_MaDatCho FOREIGN KEY (MaDatCho) REFERENCES DATCHO(MaDatCho)
GO
------ Khóa ngoại Mã khách hàng
ALTER TABLE CHITIETDATCHO ADD CONSTRAINT FK_CHITIETDATCHO_MaKhachHang FOREIGN KEY (MaKhachHang) REFERENCES KHACHHANG(MaKhachHang)
GO

------ TRIGGER: Số chi tiết đặt chỗ không lớn hơn số vé đã đặt
CREATE TRIGGER TRG_CHITIETDATCHO_SoLuongVe ON CHITIETDATCHO
FOR INSERT
AS
BEGIN
	DECLARE @soVeDaDat INT, @soVeHienTai INT, @maDatCho INT

	-- Lấy mã đặt chỗ
	SELECT @maDatCho=MaDatCho
	FROM INSERTED

	-- Lấy số vé hiện tại
	SELECT @soVeHienTai = COUNT(*)
	FROM CHITIETDATCHO
	WHERE MaDatCho=@maDatCho

	-- Lấy số vé đã đặt
	SELECT @soVeDaDat = SoVeDat
	FROM DATCHO
	WHERE MaDatCho=@maDatCho

	-- Kiểm tra số lượng vé
	IF (@soVeHienTai > @soVeDaDat)
	BEGIN
		ROLLBACK TRAN
		PRINT 'Da vuot qua so luong ve da dat cua DATCHO'
	END
END
GO

--========================================= RÀNG BUỘC BẢNG DOANH THU CHUYẾN BAY =========================================
------ Khóa ngoại Mã doanh thu tháng
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT FK_DOANHTHUCHUYENBAY_MaDoanhThuThang FOREIGN KEY (MaDoanhThuThang) REFERENCES DOANHTHUTHANG(MaDoanhThuThang)
GO
------ Khóa ngoại Mã chuyến bay
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT FK_DOANHTHUCHUYENBAY_MaChuyenBay FOREIGN KEY (MaChuyenBay) REFERENCES CHUYENBAY(MaChuyenBay)
GO
------ SoVe không được âm
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT CK_DOANHTHUCHUYENBAY_SoVe CHECK (SoVe>=0)
GO
------ DoanhThu không được âm
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT CK_DOANHTHUCHUYENBAY_DoanhThu CHECK (DoanhThu>=0)
GO
------ TiLe từ 0 - 100 %
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT CK_DOANHTHUCHUYENBAY_TiLe CHECK (TiLe>=0.0 AND TiLe<=1.0)
GO

------ DoanhThu: mặc định 0
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT DF_DOANHTHUCHUYENBAY_DoanhThu DEFAULT 0 FOR DoanhThu
GO

------ SoVe: mặc định 0
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT DF_DOANHTHUCHUYENBAY_SoVe DEFAULT 0 FOR SoVe
GO

------ TiLe: 0.0
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT DF_DOANHTHUCHUYENBAY_TiLe DEFAULT 0.0 FOR TiLe
GO

------ Trigger: Cập nhật doanh thu tháng khi có thêm doanh thu chuyến bay
CREATE TRIGGER TRG_DOANHTHUCHUYENBAY_CapNhat ON DOANHTHUCHUYENBAY
FOR INSERT
AS
BEGIN
	-- Khai báo
	DECLARE @doanhThuThem MONEY, @maDoanhThuThang INT

	-- Lấy doanh thu vừa thêm và mã doanh thu năm
	SELECT @doanhThuThem=DoanhThu, @maDoanhThuThang=MaDoanhThuThang
	FROM INSERTED

	-- Cập nhật doanh thu, số chuyến bay tháng
	UPDATE DOANHTHUTHANG
	SET DoanhThu=DoanhThu+@doanhThuThem, SoChuyenBay=SoChuyenBay+1
	WHERE MaDoanhThuThang=@maDoanhThuThang

	-- Cập nhật tỉ lệ doanh thu chuyến bay
	UPDATE DOANHTHUCHUYENBAY
	SET TiLe=DoanhThu / (
		SELECT DoanhThu
		FROM DOANHTHUTHANG dtt
		WHERE dtt.MaDoanhThuThang=@maDoanhThuThang
	)

	-- Cập nhật doanh thu, số chuyến bay năm
	UPDATE DOANHTHUNAM
	SET DoanhThu=DoanhThu+@doanhThuThem, SoChuyenBay=SoChuyenBay+1
	WHERE MaDoanhThuNam= (
		SELECT MaDoanhThuNam
		FROM DOANHTHUTHANG
		WHERE MaDoanhThuThang=@maDoanhThuThang
	)

	-- Cập nhật tỉ lệ doanh thu tháng
	UPDATE DOANHTHUTHANG
	SET TiLe=DoanhThu / (
		SELECT DoanhThu
		FROM DOANHTHUNAM
		WHERE MaDoanhThuNam= (
			SELECT MaDoanhThuNam
			FROM DOANHTHUTHANG
			WHERE MaDoanhThuThang=@maDoanhThuThang
		)
	)
END
GO

--========================================= RÀNG BUỘC BẢNG DOANH THU THÁNG =========================================
------ Khóa ngoại Mã doanh thu năm
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT FK_DOANHTHUTHANG_MaDoanhThuNam FOREIGN KEY (MaDoanhThuNam) REFERENCES DOANHTHUNAM(MaDoanhThuNam)
GO
------ Thang: 1- 12
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT CK_DOANHTHUTHANG_Thang CHECK (Thang>=1 AND Thang<= 12)
GO
------ SoChuyenBay: Không âm
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT CK_DOANHTHUTHANG_SoChuyenBay CHECK (SoChuyenBay>=0)
GO
------ DoanhThu: Không âm
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT CK_DOANHTHUTHANG_DoanhThu CHECK (DoanhThu>=0)
GO
------ TiLe: 0.00 - 100.00
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT CK_DOANHTHUTHANG_TiLe CHECK (TiLe>=0.0 AND TiLe<=1.0)
GO

------ DoanhThu: mặc định 0
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT DF_DOANHTHUTHANG_DoanhThu DEFAULT 0 FOR DoanhThu
GO

------ SoChuyenBay: mặc định 0
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT DF_DOANHTHUTHANG_SoChuyenBay DEFAULT 0 FOR SoChuyenBay
GO

------ TiLe: 0.0
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT DF_DOANHTHUTHANG_TiLe DEFAULT 0.0 FOR TiLe
GO

------ Thang: Unique với mỗi năm
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT UNIQUE_DOANHTHUTHANG_Thang UNIQUE(Thang,MaDoanhThuNam)
GO


--========================================= RÀNG BUỘC BẢNG DOANH THU NĂM =========================================
-- TRIGGER for Nam, SoChuyenBay, DoanhThu
------ Nam: <= Năm hiện tại?
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT CK_DOANHTHUNAM_Nam CHECK (Nam <= Year(GETDATE()))
GO
------ SoChuyenBay: Không âm 
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT CK_DOANHTHUNAM_SoChuyenBay CHECK (SoChuyenBay>=0)
GO
------ DoanhThu: Không âm
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT CK_DOANHTHUNAM_DoanhThu CHECK (DoanhThu>=0)
GO

------ DoanhThu: mặc định 0
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT DF_DOANHTHUNAM_DoanhThu DEFAULT 0 FOR DoanhThu
GO

------ SoChuyenBay: mặc định 0
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT DF_DOANHTHUNAM_SoChuyenBay DEFAULT 0 FOR SoChuyenBay
GO

------ Nam: UNIQUE
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT UNIQUE_DOANHTHUNAM_Nam UNIQUE(Nam)
GO

--========================================= RÀNG BUỘC BẢNG THAM SỐ =========================================
ALTER TABLE THAMSO ADD CONSTRAINT CK_THAMSO_GiaTri CHECK (GiaTri>=0)
GO

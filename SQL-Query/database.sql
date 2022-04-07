CREATE DATABASE BANVECHUYENBAY
GO

USE BANVECHUYENBAY
GO

-- Bảng Tham số
CREATE TABLE THAMSO
(
	MaThamSo INT IDENTITY(1,1) PRIMARY KEY,
	ThoiGianBayToiThieu INT NOT NULL,
	SanBayTrungGianToiDa INT NOT NULL,
	ThoiGianDungToiThieu INT NOT NULL,
	ThoiGianDungToiDa INT NOT NULL,
	ThoiGianDatVeChamNhat INT NOT NULL,
	ThoiGianHuyDatVe INT NOT NULL
)
GO

-- Bảng Chuyến bay
CREATE TABLE CHUYENBAY
(
	MaChuyenBay INT IDENTITY(1,1) PRIMARY KEY,
	GiaVe MONEY NOT NULL,
	NgayGio DATETIME NOT NULL,
	MaDuongBay INT NOT NULL,
	MaThamSo INT NOT NULL
)
GO

-- Bảng Sân bay
CREATE TABLE SANBAY
(
	MaSanBay INT IDENTITY(1,1) PRIMARY KEY,
	TenSanBay NVARCHAR(200) NOT NULL,
	TinhThanh NVARCHAR(200) NOT NULL -- Xem lai
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
	HeSo DECIMAL(5,2) NOT NULL
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
	GiaTien INT NOT NULL DEFAULT 0
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
	NgayGioDat DATETIME NOT NULL
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
	MaDoanhThuChuyenBay INT PRIMARY KEY,
	MaDoanhThuThang INT NOT NULL,
	MaChuyenBay INT NOT NULL,
	SoVe INT NOT NULL,
	DoanhThu MONEY NOT NULL,
	TiLe DECIMAL(5,2) NOT NULL
)
GO

-- Bảng Doanh thu tháng
CREATE TABLE DOANHTHUTHANG
(
	MaDoanhThuThang INT PRIMARY KEY,
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
	MaDoanhThuNam INT PRIMARY KEY,
	Nam INT NOT NULL,
	SoChuyenBay INT NOT NULL,
	DoanhThu MONEY NOT NULL
)
GO

-- RÀNG BUỘC BẢNG CHUYẾN BAY
------ Khóa ngoại Mã đường bay
ALTER TABLE CHUYENBAY ADD CONSTRAINT FK_CHUYENBAY_MaDuongBay FOREIGN KEY (MaDuongBay) REFERENCES DUONGBAY(MaDuongBay)
GO
------ Khóa ngoại Mã tham số
ALTER TABLE CHUYENBAY ADD CONSTRAINT FK_CHUYENBAY_MaThamSo FOREIGN KEY (MaThamSo) REFERENCES THAMSO(MaThamSo)
GO
------ GiaVe: không được âm
ALTER TABLE CHUYENBAY ADD CONSTRAINT CK_CHUYENBAY_GiaVe CHECK (GiaVe>=0)
GO
------ NgayGio: lớn hơn ngày giờ tại thời điểm nhận lịch chuyến bay
ALTER TABLE CHUYENBAY ADD CONSTRAINT CK_CHUYENBAY_NgayGio CHECK (NgayGio>GETDATE())
GO

-- RÀNG BUỘC BẢNG SÂN BAY TRUNG GIAN
------ Khóa ngoại Mã đường bay
ALTER TABLE SANBAYTG ADD CONSTRAINT FK_SANBAYTG_MaDuongBay FOREIGN KEY (MaDuongBay) REFERENCES DUONGBAY(MaDuongBay)
GO
------ Khóa ngoại Mã sân bay
ALTER TABLE SANBAYTG ADD CONSTRAINT FK_SANBAYTG_MaSanBay FOREIGN KEY (MaSanBay) REFERENCES SANBAY(MaSanBay)
GO
------ ThuTu: >= 1
ALTER TABLE SANBAYTG ADD CONSTRAINT CK_SANBAYTG_ThuTu CHECK (ThuTu>=1)
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
	-- Nếu ThuTu của cùng một đường bay không trùng nhau thì rollback tran
	IF ((
		SELECT COUNT(ThuTu)
		FROM SANBAYTG
		WHERE MaDuongBay=@maDuongBay AND ThuTu=@thuTu)>1)
		ROLLBACK TRAN
END
GO

ALTER TRIGGER TG_SANBAYTG_ThuTu ON SANBAYTG
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @thuTu INT, @maDuongBay INT
	-- Lấy thứ tự sân bay trung gian được thêm
	SELECT @thuTu=ThuTu, @maDuongBay=MaDuongBay
	FROM INSERTED
	-- Nếu ThuTu của cùng một đường bay không trùng nhau thì rollback tran
	-- Nếu số thứ tự == @thuTu (tính cả cái vừa thêm) > 1 thì tức là có thứ tự bị trùng
	IF ((
		SELECT COUNT(ThuTu)
		FROM SANBAYTG
		WHERE MaDuongBay=@maDuongBay AND ThuTu=@thuTu)>1)
		ROLLBACK TRAN
END
GO

------ TRIGGER ThoiGianDung: ThoiGianDung >= THAMSO.ThoiGianDungToiThieu && ThoiGianDung <= THAMSO.ThoiGianDungToiDa
CREATE TRIGGER TG_SANBAYTG_ThoiGianDung ON SANBAYTG
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @thoiGianDung INT, @maThamSo INT
	-- Lấy thời gian dừng được thêm
	SELECT @thoiGianDung=ThoiGianDung
	FROM INSERTED
	-- Lấy mã bảng tham số tương ứng với chuyến bay mà sân bay trung gian được thêm vào
	SELECT @maThamSo=CHUYENBAY.MaThamSo
	FROM CHUYENBAY, INSERTED
	WHERE CHUYENBAY.MaDuongBay=INSERTED.MaDuongBay

	-- Khai báo Thời gian dừng tối thiểu và thời gian dừng tối đa
	DECLARE @min INT, @max INT
	-- Lấy Thời gian dừng tối thiểu và thời gian dừng tối đa
	SELECT @min=ThoiGianDungToiThieu, @max=ThoiGianDungToiDa
	FROM THAMSO
	WHERE THAMSO.MaThamSo=@maThamSo

	-- Nếu Thời gian dừng không hợp lệ thì rollback tran
	IF (@thoiGianDung<@min OR @thoiGianDung>@max)
		ROLLBACK TRAN
END
GO

------ TRIGGER: So san bay TG không vượt ThamSo.SanBayTrungGianToiDa
CREATE TRIGGER TG_SANBAYTG_SanBayTrungGianToiDa ON SANBAYTG
FOR INSERT
AS
BEGIN
	DECLARE @sanBayTrungGianToiDa INT, @soSanBayTrungGianHienTai INT
	-- Lấy Sân bay trung gian tối đa
	SELECT @sanBayTrungGianToiDa=THAMSO.SanBayTrungGianToiDa
	FROM INSERTED, CHUYENBAY, THAMSO
	WHERE
		INSERTED.MaDuongBay=CHUYENBAY.MaDuongBay AND
		CHUYENBAY.MaThamSo=THAMSO.MaThamSo

	-- Lấy Số sân bay trung gian hiện tại
	SELECT @soSanBayTrungGianHienTai=COUNT(*)
	FROM SANBAYTG, INSERTED
	WHERE SANBAYTG.MaDuongBay=INSERTED.MaDuongBay

	-- Nếu số sân bay trung gian vượt thì rollback tran
	IF (@soSanBayTrungGianHienTai>@sanBayTrungGianToiDa)
		ROLLBACK TRAN
END
GO

-- RÀNG BUỘC ĐƯỜNG BAY
------ Khóa ngoại Mã sân bay đi
ALTER TABLE DUONGBAY ADD CONSTRAINT FK_DUONGBAY_MaSanBayDi FOREIGN KEY (MaSanBayDi) REFERENCES SANBAY(MaSanBay)
GO
------ Khóa ngoại Mã sân bay đến
ALTER TABLE DUONGBAY ADD CONSTRAINT FK_DUONGBAY_MaSanBayDen FOREIGN KEY (MaSanBayDen) REFERENCES SANBAY(MaSanBay)
GO
------ Trigger ThoiGianBay: ThoiGianBay >= THAMSO.ThoiGianBayToiThieu
CREATE TRIGGER TG_DUONGBAY_ThoiGianBay ON DUONGBAY
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @thoiGianBay INT, @maThamSo INT
	-- Lấy Thời gian bay được thêm
	SELECT @thoiGianBay=ThoiGianBay
	FROM INSERTED
	-- Lấy mã bảng tham số tương ứng với chuyến bay mà sân bay trung gian được thêm vào
	SELECT @maThamSo=CHUYENBAY.MaThamSo
	FROM CHUYENBAY, INSERTED
	WHERE CHUYENBAY.MaDuongBay=INSERTED.MaDuongBay

	-- Khai báo Thời gian bay tối tối thiểu
	DECLARE @min INT
	-- Lấy Thời gian dừng tối thiểu và thời gian dừng tối đa
	SELECT @min=ThoiGianBayToiThieu
	FROM THAMSO
	WHERE THAMSO.MaThamSo=@maThamSo
	-- Nếu Thời gian bay không hợp lệ thì rollback tran
	IF (@thoiGianBay<@min)
		ROLLBACK TRAN
END
GO

-- RÀNG BUỘC HẠNG VÉ
------ HeSo: 0.00 - 100.00
ALTER TABLE HANGVE ADD CONSTRAINT CK_HANGVE_HeSo CHECK (HeSo>=0.00)
GO

-- RÀNG BUỘC BẢNG CHI TIẾT HẠNG VÉ
------ Khóa ngoại Mã hạng vé
ALTER TABLE CHITIETHANGVE ADD CONSTRAINT FK_CHITIETHANGVE_MaHangVe FOREIGN KEY (MaHangVe) REFERENCES HANGVE(MaHangVe)
GO
------ Khóa ngoại Mã chuyến bay
ALTER TABLE CHITIETHANGVE ADD CONSTRAINT FK_CHITIETHANGVE_MaChuyenBay FOREIGN KEY (MaChuyenBay) REFERENCES CHUYENBAY(MaChuyenBay)
GO
------ SoGhe: lớn hơn 0
ALTER TABLE CHITIETHANGVE ADD CONSTRAINT CK_CHITIETHANGVE_SoGhe CHECK (SoGhe>0)
GO

-- RÀNG BUỘC BẢNG VÉ
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
	SET GiaTien=(@giaVe*@heSo)/100
	WHERE VE.MaVe=@maVe
END
GO

-- RÀNG BUỘC ĐẶT CHỖ
------ Khóa ngoại Mã hạng vé
ALTER TABLE DATCHO ADD CONSTRAINT FK_DATCHO_MaHangVe FOREIGN KEY (MaHangVe) REFERENCES HANGVE(MaHangVe)
GO
------ Khóa ngoại Mã chuyến bay
ALTER TABLE DATCHO ADD CONSTRAINT FK_DATCHO_MaChuyenBay FOREIGN KEY (MaChuyenBay) REFERENCES CHUYENBAY(MaChuyenBay)
GO
------ Khóa ngoại Mã người đặt
ALTER TABLE DATCHO ADD CONSTRAINT FK_DATCHO_MaNguoiDat FOREIGN KEY (MaNguoiDat) REFERENCES KHACHHANG(MaKhachHang)
GO
------ Trigger NgayGioDat: NgayGioDat <= (Hiện tại - THAMSO.ThoiGianDatVeChamNhat)
CREATE TRIGGER TG_DATCHO_NgayGioDat ON DATCHO
FOR INSERT, UPDATE
AS
BEGIN
	-- Khai báo
	DECLARE @ngayGioDat DATETIME, @maThamSo INT
	-- Lấy ngày giờ đăt chỗ được thêm
	SELECT @ngayGioDat=NgayGioDat
	FROM INSERTED

	-- Lấy mã bảng tham số tương ứng với chuyến bay mà sân bay trung gian được thêm vào
	SELECT @maThamSo=CHUYENBAY.MaThamSo
	FROM CHUYENBAY, INSERTED
	WHERE CHUYENBAY.MaChuyenBay=INSERTED.MaChuyenBay

	-- Khai báo ThoiGianDatVeChamNhat
	DECLARE @datVeChamNhat INT
	-- Lấy Thời gian dừng tối thiểu và thời gian dừng tối đa
	SELECT @datVeChamNhat=ThoiGianDatVeChamNhat
	FROM THAMSO
	WHERE THAMSO.MaThamSo=@maThamSo

	-- Nếu ngày giờ đặt chỗ không hợp lệ thì rollback tran
	IF (@ngayGioDat<=(DATEADD(day, -@datVeChamNhat, GETDATE())))
		ROLLBACK TRAN
END
GO

-- RÀNG BUỘC BẢNG CHI TIẾT ĐẶT CHỖ
------ Khóa ngoại Mã đặt chỗ
ALTER TABLE CHITIETDATCHO ADD CONSTRAINT FK_CHITIETDATCHO_MaDatCho FOREIGN KEY (MaDatCho) REFERENCES DATCHO(MaDatCho)
GO
------ Khóa ngoại Mã khách hàng
ALTER TABLE CHITIETDATCHO ADD CONSTRAINT FK_CHITIETDATCHO_MaKhachHang FOREIGN KEY (MaKhachHang) REFERENCES KHACHHANG(MaKhachHang)
GO

-- RÀNG BUỘC BẢNG DOANH THU CHUYẾN BAY
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
ALTER TABLE DOANHTHUCHUYENBAY ADD CONSTRAINT CK_DOANHTHUCHUYENBAY_TiLe CHECK (TiLe>=0.00 AND TiLe<=100.00)
GO

-- RÀNG BUỘC BẢNG DOANH THU THÁNG
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
ALTER TABLE DOANHTHUTHANG ADD CONSTRAINT CK_DOANHTHUTHANG_TiLe CHECK (TiLe>=0.00 AND TiLe<=100.00)
GO

-- RÀNG BUỘC BẢNG DOANH THU NĂM
-- TRIGGER for Nam, SoChuyenBay, DoanhThu
------ Nam: 
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT CK_DOANHTHUNAM_Nam CHECK (Nam >= 1 AND Nam<= 12)
GO
------ SoChuyenBay: Không âm
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT CK_DOANHTHUNAM_SoChuyenBay CHECK (SoChuyenBay>=0)
GO
------ DoanhThu: Không âm
ALTER TABLE DOANHTHUNAM ADD CONSTRAINT CK_DOANHTHUNAM_DoanhThu CHECK (DoanhThu>=0)

------ TRIGGER: Nam <= năm hiện tại ?? coi lại
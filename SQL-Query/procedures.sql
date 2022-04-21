CREATE PROC GetFlightData 
AS
BEGIN
	SELECT 
		cb.MaChuyenBay,
		sb1.TenSanBay AS [SanBayDi],
		sb2.TenSanBay AS [SanBayDen],
		cb.NgayGio,
		COUNT(DISTINCT sbtg.MaSanBayTG) AS [SoDiemDung],
		COUNT(DISTINCT cthv.MaCTHV) AS [SoLuongVe],
		cb.GiaVe,
		SUM(cthv.SoGhe) - 
		(
			SELECT ISNULL(COUNT(*), 0)
			FROM CHUYENBAY JOIN VE ON (CHUYENBAY.MaChuyenBay=VE.MaChuyenBay)
			WHERE CHUYENBAY.MaChuyenBay=cb.MaChuyenBay
		) - 
		(
			SELECT ISNULL(SUM(SoVeDat), 0)
			FROM CHUYENBAY JOIN DATCHO ON (CHUYENBAY.MaChuyenBay=DATCHO.MaChuyenBay)
			WHERE CHUYENBAY.MaChuyenBay=cb.MaChuyenBay
		) 
		AS [GheTrong]
	FROM 
		CHUYENBAY cb JOIN
		DUONGBAY db ON (cb.MaDuongBay=db.MaDuongBay) JOIN
		SANBAY sb1 ON (db.MaSanBayDi=sb1.MaSanBay) JOIN
		SANBAY sb2 ON (db.MaSanBayDen=sb2.MaSanBay) JOIN
		SANBAYTG sbtg ON (db.MaDuongBay=sbtg.MaDuongBay) JOIN
		CHITIETHANGVE cthv ON (cb.MaChuyenBay=cthv.MaChuyenBay)	
	GROUP BY cb.MaChuyenBay, sb1.TenSanBay, sb2.TenSanBay, NgayGio, cb.GiaVe

END
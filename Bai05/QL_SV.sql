CREATE TABLE SINHVIEN(
	MASV VARCHAR(10) PRIMARY KEY,
	TENSV NVARCHAR(50),
	KHOA NVARCHAR(50),
	DIEMTB FLOAT
);

INSERT INTO SINHVIEN (MASV, TENSV, KHOA, DIEMTB)
VALUES
('SV001', N'Nguyễn Văn A', N'Công nghệ thông tin', 8.2),
('SV002', N'Trần Thị B', N'Hệ thống thông tin', 7.6),
('SV003', N'Lê Văn C', N'Khoa học máy tính', 6.8),
('SV004', N'Phạm Thị D', N'Công nghệ thông tin', 9.1),
('SV005', N'Hoàng Minh E', N'Công nghệ phần mềm', 7.0),
('SV006', N'Đỗ Quốc F', N'Kỹ thuật máy tính', 5.9),
('SV007', N'Vũ Thị G', N'Trí tuệ nhân tạo', 8.7),
('SV008', N'Phan Xuân H', N'Khoa học dữ liệu', 7.8),
('SV009', N'Bùi Nhật I', N'Hệ thống thông tin', 8.0),
('SV010', N'Huỳnh Mỹ J', N'Công nghệ thông tin', 6.5)

ALTER TABLE SinhVien
ADD CONSTRAINT CK_TENSV 
CHECK (LEN(LTRIM(RTRIM(TENSV))) > 0)

ALTER TABLE SinhVien
ADD CONSTRAINT CK_DIEMTB
CHECK (DIEMTB >= 0 AND DIEMTB <= 10)
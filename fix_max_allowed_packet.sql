-- Script để fix lỗi "Packets larger than max_allowed_packet"
-- Chạy script này trong MySQL để tăng max_allowed_packet lên 64MB

-- Kiểm tra giá trị hiện tại
SHOW VARIABLES LIKE 'max_allowed_packet';

-- Tăng max_allowed_packet lên 64MB (67108864 bytes)
-- Lưu ý: Cần quyền SUPER hoặc quyền admin
SET GLOBAL max_allowed_packet = 67108864;

-- Verify lại
SHOW VARIABLES LIKE 'max_allowed_packet';

-- Lưu ý: 
-- 1. SET GLOBAL chỉ có hiệu lực cho session mới
-- 2. Để vĩnh viễn, cần sửa file my.ini/my.cnf và restart MySQL
-- 3. Xem file FIX_MAX_ALLOWED_PACKET.md để biết cách cấu hình vĩnh viễn

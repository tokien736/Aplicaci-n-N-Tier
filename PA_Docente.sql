--------------------------------------------------------------------------------------------
if OBJECT_ID('spListarDocente') is not null
	drop proc spListarDocente
go
create proc spListarDocente
as
begin
	select CodDocente, APaterno, AMaterno, Nombres, CodUsuario
	from TDocente
end
go
--------------------------------------------------------------------------------------------
if OBJECT_ID('spAgregarDocente') is not null
	drop proc spAgregarDocente
go
create proc spAgregarDocente
	@CodDocente char(3),
	@APaterno varchar(50),
	@AMaterno varchar(50),
	@Nombres varchar(50),
	@CodUsuario varchar(50),
	@Contrasena varchar(50)
as
begin
	-- Verificar que el código de docente no esté duplicado
	if not exists(select CodDocente from TDocente where CodDocente = @CodDocente)
		-- Verificar que el usuario no esté asignado en TUsuario
		if not exists(select CodUsuario from TUsuario where CodUsuario = @CodUsuario)
		begin
			declare @EncryptedPassword varbinary(256)
			set @EncryptedPassword = ENCRYPTBYPASSPHRASE('miFraseDeContraseña', @Contrasena)

			begin tran tranAgregar 
			begin try
				-- Insertar el usuario en TUsuario
				insert into TUsuario values(@CodUsuario, @EncryptedPassword)
				-- Insertar el docente en TDocente
				insert into TDocente values(@CodDocente, @APaterno, @AMaterno, @Nombres, @CodUsuario)
				commit tran tranAgregar
				select CodError = 0, Mensaje = 'Docente insertado correctamente'
			end try
			begin catch
				rollback tran tranAgregar
				select CodError = 1, Mensaje = 'Error: No se ejecutó la transacción'
			end catch
		end
		else select CodError = 1, Mensaje = 'Error: Usuario ya asignado en TUsuario'
	else select CodError = 1, Mensaje = 'Error: Código de docente duplicado en TDocente'
end
go

--------------------------------------------------------------------------------------------
if OBJECT_ID('spEliminarDocente') is not null
    drop proc spEliminarDocente
go

create proc spEliminarDocente
    @CodDocente char(3)
as
begin
    if exists (select 1 from TDocente where CodDocente = @CodDocente)
    begin
        begin try
            -- Eliminar registros de TCarga relacionados con el docente
            delete from TCarga where CodDocente = @CodDocente;

            -- Eliminar el docente
            delete from TDocente where CodDocente = @CodDocente;

            select 'Docente y registros en TCarga eliminados exitosamente' as Mensaje
        end try
        begin catch
            select 'Error al eliminar el docente y sus registros en TCarga' as Mensaje
        end catch
    end
    else
    begin
        select 'Docente no encontrado' as Mensaje
    end
end
go



--------------------------------------------------------------------------------------------
if OBJECT_ID('spActualizarDocente') is not null
	drop proc spActualizarDocente
go
create proc spActualizarDocente
	@CodDocente char(3),
	@APaterno varchar(50),
	@AMaterno varchar(50),
	@Nombres varchar(50)
as
begin
	if exists (select 1 from TDocente where CodDocente = @CodDocente)
	begin
		update TDocente
		set APaterno = @APaterno,
			AMaterno = @AMaterno,
			Nombres = @Nombres
		where CodDocente = @CodDocente
		select 'Docente actualizado exitosamente' as Mensaje
	end
	else
	begin
		select 'Docente no encontrado' as Mensaje
	end
end
go
--------------------------------------------------------------------------------------------
if OBJECT_ID('spBuscarDocentePorApellido') is not null
	drop proc spBuscarDocentePorApellido
go
create proc spBuscarDocentePorApellido
	@APaterno varchar(50)
as
begin
	select CodDocente, APaterno, AMaterno, Nombres, CodUsuario
	from TDocente
	where APaterno like '%' + @APaterno + '%'
end
go
--------------------------------------------------------------------------------------------
-- Ejemplo de ejecución del procedimiento almacenado spAgregarDocente con valores de prueba
exec spAgregarDocente 'D09', 'González', 'López', 'Juan', 'jgonzalez', 'password123';


EXEC spListarDocente;
GO
EXEC spEliminarDocente @CodDocente = 'D01';
GO
EXEC spActualizarDocente 
    @CodDocente = 'D03', 
    @APaterno = 'NuevoPaterno', 
    @AMaterno = 'NuevoMaterno', 
    @Nombres = 'NuevoNombre'
GO
EXEC spBuscarDocentePorApellido @APaterno = 'nina';
GO
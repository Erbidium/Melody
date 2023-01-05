﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Melody.Infrastructure.Data {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SqlScriptsResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SqlScriptsResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Melody.Infrastructure.Data.SqlScriptsResource", typeof(SqlScriptsResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO UserPlaylists (UserId, PlaylistId)
        ///VALUES (@UserId, @Id).
        /// </summary>
        internal static string CreateFavouritePlaylist {
            get {
                return ResourceManager.GetString("CreateFavouritePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO FavouriteSongs (UserId, SongId)
        ///VALUES (@UserId, @Id).
        /// </summary>
        internal static string CreateFavouriteSong {
            get {
                return ResourceManager.GetString("CreateFavouriteSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Playlists (Name, AuthorId)
        ///OUTPUT Inserted.Id
        ///VALUES (@Name, @AuthorId).
        /// </summary>
        internal static string CreatePlaylist {
            get {
                return ResourceManager.GetString("CreatePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO RecommendationsPreferences(UserId, AuthorName, StartYear, EndYear, GenreId, AverageDurationInMinutes)
        ///VALUES (@UserId, @AuthorName, @StartYear, @EndYear, @GenreId, @AverageDurationInMinutes);.
        /// </summary>
        internal static string CreateRecommendationsPreferences {
            get {
                return ResourceManager.GetString("CreateRecommendationsPreferences", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO UserRefreshTokens(UserId, RefreshToken)
        ///VALUES (@UserId, @Token);.
        /// </summary>
        internal static string CreateRefreshToken {
            get {
                return ResourceManager.GetString("CreateRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Roles
        ///VALUES (@Name, @NormalizedName).
        /// </summary>
        internal static string CreateRole {
            get {
                return ResourceManager.GetString("CreateRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Songs (UserId, Name, Path, AuthorName, Year, SizeBytes, UploadedAt, GenreId, Duration)
        ///OUTPUT Inserted.Id
        ///VALUES (@UserId, @Name, @Path, @AuthorName, @Year, @SizeBytes, @UploadedAt, @GenreId, @Duration).
        /// </summary>
        internal static string CreateSong {
            get {
                return ResourceManager.GetString("CreateSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Users (UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, PhoneNumber)
        ///OUTPUT Inserted.Id
        ///VALUES (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @PhoneNumber)
        ///.
        /// </summary>
        internal static string CreateUser {
            get {
                return ResourceManager.GetString("CreateUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///From UserPlaylists
        ///WHERE UserId = @UserId
        ///  AND PlaylistId = @Id.
        /// </summary>
        internal static string DeleteFavouritePlaylist {
            get {
                return ResourceManager.GetString("DeleteFavouritePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///From FavouriteSongs
        ///WHERE UserId = @UserId
        ///  AND SongId = @Id.
        /// </summary>
        internal static string DeleteFavouriteSong {
            get {
                return ResourceManager.GetString("DeleteFavouriteSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Playlists
        ///SET IsDeleted = 1
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string DeletePlaylist {
            get {
                return ResourceManager.GetString("DeletePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///From PlaylistSongs
        ///WHERE PlaylistId = @Id
        ///  AND SongId = @SongId.
        /// </summary>
        internal static string DeletePlaylistSong {
            get {
                return ResourceManager.GetString("DeletePlaylistSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///FROM UserRefreshTokens
        ///WHERE RefreshToken = @Token;.
        /// </summary>
        internal static string DeleteRefreshToken {
            get {
                return ResourceManager.GetString("DeleteRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///FROM UserRefreshTokens
        ///WHERE UserId = @UserId;.
        /// </summary>
        internal static string DeleteRefreshTokenByUserId {
            get {
                return ResourceManager.GetString("DeleteRefreshTokenByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///FROM Roles
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string DeleteRole {
            get {
                return ResourceManager.GetString("DeleteRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///FROM UserRoles
        ///WHERE UserId = @UserId;.
        /// </summary>
        internal static string DeleteRoles {
            get {
                return ResourceManager.GetString("DeleteRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Songs
        ///SET IsDeleted = 1
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string DeleteSong {
            get {
                return ResourceManager.GetString("DeleteSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Songs
        ///SET IsDeleted = 1
        ///WHERE Id = @Id
        ///  AND UserId = @UserId
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string DeleteUploadedSong {
            get {
                return ResourceManager.GetString("DeleteUploadedSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Users
        ///SET IsDeleted = 1
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0;.
        /// </summary>
        internal static string DeleteUser {
            get {
                return ResourceManager.GetString("DeleteUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, UserId, RefreshToken
        ///FROM UserRefreshTokens
        ///WHERE UserId = @UserId;.
        /// </summary>
        internal static string FindRefreshTokenByUserId {
            get {
                return ResourceManager.GetString("FindRefreshTokenByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, UserId, RefreshToken
        ///FROM UserRefreshTokens
        ///WHERE RefreshToken = @Token;.
        /// </summary>
        internal static string FindRefreshTokenByValue {
            get {
                return ResourceManager.GetString("FindRefreshTokenByValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name
        ///FROM Genres.
        /// </summary>
        internal static string GetAllGenres {
            get {
                return ResourceManager.GetString("GetAllGenres", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, AuthorId, IsDeleted
        ///FROM Playlists p
        ///         INNER JOIN Users u ON u.Id = p.AuthorId
        ///WHERE u.IsDeleted = 0
        ///  AND u.IsBanned = 0
        ///  AND p.IsDeleted = 0.
        /// </summary>
        internal static string GetAllPlaylists {
            get {
                return ResourceManager.GetString("GetAllPlaylists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF ISNULL(@SearchText, &apos;&apos;) = &apos;&apos; SET @SearchText = &apos;&quot;&quot;&apos;;
        ///
        ///SELECT Songs.Id,
        ///       UserId,
        ///       UploadedAt,
        ///       SizeBytes,
        ///       Songs.Name,
        ///       Songs.Path,
        ///       Songs.AuthorName,
        ///       Songs.Year,
        ///       Songs.GenreId,
        ///       Songs.Duration,
        ///       Songs.IsDeleted,
        ///       Genres.Id,
        ///       Genres.Name
        ///FROM Songs
        ///         INNER JOIN Genres ON Songs.GenreId = Genres.Id
        ///         INNER JOIN Users u ON u.Id = Songs.UserId
        ///WHERE u.IsDeleted = 0
        ///  AND Songs.IsDeleted = 0
        ///  AND (@Sear [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetAllSongs {
            get {
                return ResourceManager.GetString("GetAllSongs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT s.Id,
        ///       s.UserId,
        ///       UploadedAt,
        ///       SizeBytes,
        ///       s.Name,
        ///       Path,
        ///       AuthorName,
        ///       Year,
        ///       GenreId,
        ///       Duration,
        ///       IsDeleted,
        ///       g.Id,
        ///       g.Name
        ///FROM Songs s
        ///         INNER JOIN Genres g ON s.GenreId = g.Id
        ///         INNER JOIN FavouriteSongs fs ON fs.SongId = s.Id
        ///WHERE s.IsDeleted = 0
        ///  AND fs.UserId = @UserId
        ///UNION
        ///SELECT s.Id,
        ///       s.UserId,
        ///       s.UploadedAt,
        ///       s.SizeBytes,
        ///       s.Name,
        ///       s.Path,
        ///        [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetFavouriteAndUploadedUserSongs {
            get {
                return ResourceManager.GetString("GetFavouriteAndUploadedUserSongs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT p.Id,
        ///       p.Name,
        ///       p.AuthorId,
        ///       p.IsDeleted,
        ///       ps.Id,
        ///       ps.UploadedAt,
        ///       ps.Name,
        ///       ps.AuthorName,
        ///       ps.GenreId,
        ///       ps.Duration,
        ///       ps.IsDeleted,
        ///       ps.IsFavourite
        ///FROM Playlists p
        ///         LEFT JOIN
        ///     (SELECT s.Id,
        ///             s.UploadedAt,
        ///             s.Name,
        ///             s.AuthorName,
        ///             s.GenreId,
        ///             s.Duration,
        ///             s.IsDeleted,
        ///             ps.PlaylistId,
        ///             CONVERT(BIT, IIF(fs.S [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetFavouritePlaylists {
            get {
                return ResourceManager.GetString("GetFavouritePlaylists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT s.Id,
        ///       s.UserId,
        ///       s.UploadedAt,
        ///       s.SizeBytes,
        ///       s.Name,
        ///       s.Path,
        ///       s.AuthorName,
        ///       s.Year,
        ///       s.GenreId,
        ///       s.Duration,
        ///       s.IsDeleted,
        ///       g.Id,
        ///       g.Name
        ///FROM Songs s
        ///         INNER JOIN Genres g ON s.GenreId = g.Id
        ///         INNER JOIN FavouriteSongs fs ON fs.SongId = s.Id
        ///         INNER JOIN Users u ON u.Id = fs.UserId
        ///WHERE s.IsDeleted = 0
        ///  AND fs.UserId = @UserId
        ///  AND u.IsDeleted = 0
        ///  AND u.IsBanned = 0
        ///ORDER BY s. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetFavouriteSongs {
            get {
                return ResourceManager.GetString("GetFavouriteSongs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name
        ///FROM Genres
        ///WHERE Id = @Id.
        /// </summary>
        internal static string GetGenreById {
            get {
                return ResourceManager.GetString("GetGenreById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT p.Id,
        ///       p.Name,
        ///       p.AuthorId,
        ///       CONVERT(BIT, IIF(up.PlaylistId IS NULL, 0, 1)) as IsFavourite,
        ///       p.IsDeleted,
        ///       ps.Id,
        ///       ps.UploadedAt,
        ///       ps.Name,
        ///       ps.AuthorName,
        ///       ps.GenreId,
        ///       ps.Duration,
        ///       ps.IsDeleted,
        ///       ps.IsFavourite,
        ///       ps.GenreId                                     as Id,
        ///       ps.GenreName                                   as Name
        ///FROM Playlists p
        ///         LEFT JOIN
        ///     (SELECT s.Id,
        ///             s.Uploade [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetPlaylistById {
            get {
                return ResourceManager.GetString("GetPlaylistById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT p.Id,
        ///       p.Name,
        ///       p.AuthorId,
        ///       CONVERT(BIT, IIF(up.PlaylistId IS NULL, 0, 1)) as IsFavourite,
        ///       p.IsDeleted,
        ///       ps.Id,
        ///       ps.UploadedAt,
        ///       ps.Name,
        ///       ps.AuthorName,
        ///       ps.GenreId,
        ///       ps.Duration,
        ///       ps.IsDeleted,
        ///       ps.IsFavourite
        ///FROM Playlists p
        ///         LEFT JOIN
        ///     (SELECT s.Id,
        ///             s.UploadedAt,
        ///             s.Name,
        ///             s.AuthorName,
        ///             s.GenreId,
        ///             s.Duration,
        ///             s.IsDe [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetPlaylistsCreatedByUserId {
            get {
                return ResourceManager.GetString("GetPlaylistsCreatedByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT UserId,
        ///       AuthorName,
        ///       StartYear,
        ///       EndYear,
        ///       GenreId,
        ///       AverageDurationInMinutes
        ///FROM RecommendationsPreferences
        ///WHERE UserId = @UserId;.
        /// </summary>
        internal static string GetRecommendationsPreferences {
            get {
                return ResourceManager.GetString("GetRecommendationsPreferences", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, NormalizedName
        ///FROM Roles
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string GetRoleById {
            get {
                return ResourceManager.GetString("GetRoleById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, NormalizedName
        ///FROM Roles
        ///WHERE NormalizedName = @NormalizedName;.
        /// </summary>
        internal static string GetRoleByName {
            get {
                return ResourceManager.GetString("GetRoleByName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Roles.Id, Roles.Name, Roles.NormalizedName
        ///FROM Roles
        ///         INNER JOIN UserRoles ON UserRoles.RoleId = Roles.Id
        ///         INNER JOIN Users ON UserRoles.UserId = Users.Id
        ///WHERE Users.Id = @UserId;.
        /// </summary>
        internal static string GetRoles {
            get {
                return ResourceManager.GetString("GetRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT s.Id,
        ///       s.UserId,
        ///       s.UploadedAt,
        ///       s.SizeBytes,
        ///       s.Name,
        ///       s.Path,
        ///       s.AuthorName,
        ///       s.Year,
        ///       s.GenreId,
        ///       s.Duration,
        ///       s.IsDeleted
        ///FROM Songs s
        ///         INNER JOIN Users u ON u.Id = s.UserId
        ///WHERE s.Id = @Id
        ///  AND s.IsDeleted = 0
        ///  AND u.IsDeleted = 0
        ///  AND u.IsBanned = 0
        ///
        ///.
        /// </summary>
        internal static string GetSongById {
            get {
                return ResourceManager.GetString("GetSongById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT s.Id,
        ///       s.UserId,
        ///       s.UploadedAt,
        ///       s.SizeBytes,
        ///       s.Name,
        ///       s.Path,
        ///       s.AuthorName,
        ///       s.Year,
        ///       s.GenreId,
        ///       s.Duration,
        ///       s.IsDeleted,
        ///       CONVERT(BIT, IIF(fs.SongId IS NULL, 0, 1)) as IsFavourite,
        ///       g.Id,
        ///       g.Name
        ///FROM Songs s
        ///         INNER JOIN Genres g ON s.GenreId = g.Id
        ///         INNER JOIN Users u ON u.Id = s.UserId
        ///         LEFT JOIN
        ///     (SELECT SongId
        ///      FROM FavouriteSongs fs
        ///      WHERE fs.UserId = @Use [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetSongsByIds {
            get {
                return ResourceManager.GetString("GetSongsByIds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WITH PlaylistSongsIds AS
        ///         (SELECT s.Id
        ///          FROM Songs s
        ///                   INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
        ///                   INNER JOIN Playlists p ON ps.PlaylistId = p.Id
        ///          WHERE s.IsDeleted = 0
        ///            AND p.IsDeleted = 0
        ///            AND p.Id = @PlaylistId)
        ///SELECT s.Id,
        ///       s.UserId,
        ///       s.UploadedAt,
        ///       s.SizeBytes,
        ///       s.Name,
        ///       s.Path,
        ///       s.AuthorName,
        ///       s.Year,
        ///       s.GenreId,
        ///       s.Duration,
        ///       s.IsDelete [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetSongsToAddToPlaylist {
            get {
                return ResourceManager.GetString("GetSongsToAddToPlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT s.Id,
        ///       s.UserId,
        ///       s.UploadedAt,
        ///       s.SizeBytes,
        ///       s.Name,
        ///       s.Path,
        ///       s.AuthorName,
        ///       s.Year,
        ///       s.GenreId,
        ///       s.Duration,
        ///       s.IsDeleted,
        ///       Genres.Id,
        ///       Genres.Name
        ///FROM Songs s
        ///         INNER JOIN Genres ON s.GenreId = Genres.Id
        ///WHERE s.IsDeleted = 0
        ///  AND s.UserId = @UserId
        ///ORDER BY s.UploadedAt DESC
        ///OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY.
        /// </summary>
        internal static string GetSongsUploadedByUserId {
            get {
                return ResourceManager.GetString("GetSongsUploadedByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id,
        ///       UserName,
        ///       NormalizedUserName,
        ///       Email,
        ///       NormalizedEmail,
        ///       EmailConfirmed,
        ///       PasswordHash,
        ///       PhoneNumber,
        ///       IsBanned,
        ///       IsDeleted
        ///FROM Users
        ///WHERE NormalizedEmail = @NormalizedEmail.
        /// </summary>
        internal static string GetUserByEmail {
            get {
                return ResourceManager.GetString("GetUserByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id,
        ///       UserName,
        ///       NormalizedUserName,
        ///       Email,
        ///       NormalizedEmail,
        ///       EmailConfirmed,
        ///       PasswordHash,
        ///       PhoneNumber,
        ///       IsBanned,
        ///       IsDeleted
        ///FROM Users
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string GetUserById {
            get {
                return ResourceManager.GetString("GetUserById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id,
        ///       UserName,
        ///       NormalizedUserName,
        ///       Email,
        ///       NormalizedEmail,
        ///       EmailConfirmed,
        ///       PasswordHash,
        ///       PhoneNumber,
        ///       IsBanned,
        ///       IsDeleted
        ///FROM Users
        ///WHERE NormalizedUserName = @NormalizedUserName;.
        /// </summary>
        internal static string GetUserByName {
            get {
                return ResourceManager.GetString("GetUserByName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT UserId, RoleId
        ///FROM UserRoles
        ///WHERE UserId = @UserId
        ///  AND RoleId = @RoleId;.
        /// </summary>
        internal static string GetUserRoles {
            get {
                return ResourceManager.GetString("GetUserRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Users.Id,
        ///       Users.UserName,
        ///       Users.NormalizedUserName,
        ///       Users.Email,
        ///       Users.NormalizedEmail,
        ///       Users.EmailConfirmed,
        ///       Users.PasswordHash,
        ///       Users.PhoneNumber,
        ///       Users.IsBanned,
        ///       Users.IsDeleted
        ///FROM Users
        ///         INNER JOIN Roles ON Users.RoleId = Roles.Id
        ///WHERE Roles.NormalizedName = @NormalizedName
        ///  AND Users.IsDeleted = 0;.
        /// </summary>
        internal static string GetUsersInRole {
            get {
                return ResourceManager.GetString("GetUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IF ISNULL(@SearchText, &apos;&apos;) = &apos;&apos; SET @SearchText = &apos;&quot;&quot;&apos;;
        ///
        ///SELECT Users.Id,
        ///       Users.UserName,
        ///       Users.NormalizedUserName,
        ///       Users.Email,
        ///       Users.NormalizedEmail,
        ///       Users.EmailConfirmed,
        ///       Users.PasswordHash,
        ///       Users.PhoneNumber,
        ///       Users.IsBanned,
        ///       Users.IsDeleted
        ///FROM Users
        ///WHERE NOT EXISTS(
        ///        SELECT u.Id
        ///        FROM Users u
        ///                 INNER JOIN UserRoles ur ON ur.UserId = u.Id
        ///        WHERE ur.RoleId = 1
        ///          AND ur.UserId = U [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetUsersWithoutAdminRole {
            get {
                return ResourceManager.GetString("GetUsersWithoutAdminRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT SUM(SizeBytes)
        ///FROM Songs
        ///WHERE UserId = @UserId
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string GetUserTotalUploadsSize {
            get {
                return ResourceManager.GetString("GetUserTotalUploadsSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO PlaylistSongs (PlaylistId, SongId)
        ///VALUES (@PlaylistId, @SongId).
        /// </summary>
        internal static string InsertPlaylistSong {
            get {
                return ResourceManager.GetString("InsertPlaylistSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO UserRoles (UserId, RoleId)
        ///VALUES (@UserId, @RoleId);.
        /// </summary>
        internal static string InsertRoles {
            get {
                return ResourceManager.GetString("InsertRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO ListeningStatistics (SongId, UserId, Date)
        ///VALUES (@Id, @UserId, @Date).
        /// </summary>
        internal static string SaveNewSongListening {
            get {
                return ResourceManager.GetString("SaveNewSongListening", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Users
        ///SET IsBanned = @IsBanned
        ///WHERE Id = @UserId
        ///  AND IsDeleted = 0;.
        /// </summary>
        internal static string SetUserBanStatus {
            get {
                return ResourceManager.GetString("SetUserBanStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE RecommendationsPreferences
        ///SET AuthorName               = @AuthorName,
        ///    StartYear                = @StartYear,
        ///    EndYear                  = @EndYear,
        ///    GenreId                  = @GenreId,
        ///    AverageDurationInMinutes = @AverageDurationInMinutes
        ///WHERE UserId = @UserId;.
        /// </summary>
        internal static string UpdateRecommendationsPreferences {
            get {
                return ResourceManager.GetString("UpdateRecommendationsPreferences", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE UserRefreshTokens
        ///SET RefreshToken = @Token
        ///WHERE UserId = @Id;.
        /// </summary>
        internal static string UpdateRefreshToken {
            get {
                return ResourceManager.GetString("UpdateRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Roles
        ///SET Name           = @Name,
        ///    NormalizedName = @NormalizedName
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string UpdateRole {
            get {
                return ResourceManager.GetString("UpdateRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Songs
        ///SET UserId     = @UserId,
        ///    Name       = @Name,
        ///    Path       = @Path,
        ///    AuthorName = @AuthorName,
        ///    Year       = @Year,
        ///    SizeBytes  = @SizeBytes,
        ///    UploadedAt = @UploadedAt,
        ///    GenreId    = @GenreId,
        ///    Duration   = @Duration
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string UpdateSong {
            get {
                return ResourceManager.GetString("UpdateSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Users
        ///SET UserName           = @UserName,
        ///    NormalizedUserName = @NormalizedUserName,
        ///    Email              = @Email,
        ///    NormalizedEmail    = @NormalizedEmail,
        ///    EmailConfirmed     = @EmailConfirmed,
        ///    PasswordHash       = @PasswordHash,
        ///    PhoneNumber        = @PhoneNumber,
        ///    IsBanned           = @IsBanned,
        ///    IsDeleted          = @IsDeleted
        ///WHERE Id = @Id
        ///  AND Users.IsDeleted = 0;.
        /// </summary>
        internal static string UpdateUser {
            get {
                return ResourceManager.GetString("UpdateUser", resourceCulture);
            }
        }
    }
}

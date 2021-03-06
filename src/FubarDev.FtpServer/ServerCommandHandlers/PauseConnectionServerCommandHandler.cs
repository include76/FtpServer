// <copyright file="PauseConnectionServerCommandHandler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

using FubarDev.FtpServer.Features;
using FubarDev.FtpServer.ServerCommands;

using Microsoft.Extensions.Logging;

namespace FubarDev.FtpServer.ServerCommandHandlers
{
    /// <summary>
    /// Handler for the <see cref="PauseConnectionServerCommand"/>.
    /// </summary>
    public class PauseConnectionServerCommandHandler : IServerCommandHandler<PauseConnectionServerCommand>
    {
        private readonly IFtpConnectionAccessor _connectionAccessor;

        private readonly ILogger<PauseConnectionServerCommandHandler>? _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PauseConnectionServerCommandHandler"/> class.
        /// </summary>
        /// <param name="connectionAccessor">The FTP connection accessor.</param>
        /// <param name="logger">The logger.</param>
        public PauseConnectionServerCommandHandler(
            IFtpConnectionAccessor connectionAccessor,
            ILogger<PauseConnectionServerCommandHandler>? logger = null)
        {
            _connectionAccessor = connectionAccessor;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(PauseConnectionServerCommand command, CancellationToken cancellationToken)
        {
            var connection = _connectionAccessor.FtpConnection;
            var networkStreamFeature = connection.Features.Get<INetworkStreamFeature>();

            await networkStreamFeature.SecureConnectionAdapter.Receiver.PauseAsync(cancellationToken)
               .ConfigureAwait(false);
            _logger?.LogDebug("Receiver paused");
        }
    }
}

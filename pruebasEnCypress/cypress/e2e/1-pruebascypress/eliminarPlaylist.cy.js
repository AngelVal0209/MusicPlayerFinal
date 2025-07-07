describe('eliminarPlaylist', () => {
  it('eliminarPlaylist', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').type('test@test.com');
    cy.get('#Contrasena').click();
    cy.get('#Contrasena').type('test123');
    cy.get('.btn-login').click();
    cy.contains('Musica').click();
    cy.contains('Volver a Mis Playlists').click();
    cy.get('button.btn-danger').contains('Eliminar').click();
    cy.get('.btn-danger:nth-child(1)').click();
    cy.get('div.alert-success').should('contain.text', '¡Playlist eliminada exitosamente!');
  });
});